// See the Electron documentation for details on how to use preload scripts:
// https://www.electronjs.org/docs/latest/tutorial/process-model#preload-scripts
import path from 'path';

import os from 'os';

import {spawn, exec} from 'child_process';
import {ipcRenderer} from 'electron';

const platform = os.platform();
const isBackendSpawned = () => {
	let cmd = '';
	switch (platform) {
		case 'win32':
			cmd = `tasklist`;
			break;
		case 'darwin':
			cmd = `ps -ax | grep RetireSimple.Backend`;
			break;
		case 'linux':
			cmd = `ps -A`;
			break;
		default:
			break;
	}

	let exec_result = false;
	const callback = (result: boolean) => {
		exec_result = result;
	};

	exec(cmd, (err, stdout) => {
		callback(stdout.toLowerCase().indexOf('retireSimple.backend') > -1);
	});
	return exec_result;
};

const binName =
	platform === 'darwin' || platform === 'linux'
		? 'RetireSimple.Backend'
		: 'RetireSimple.Backend.exe';

const binPath =
	process.env.NODE_ENV === 'development'
		? path.join(__dirname, '..', 'resources', binName)
		: path.join(process.resourcesPath, binName);
const cwdPath =
	process.env.NODE_ENV === 'development'
		? path.join(__dirname, '..', 'resources')
		: process.resourcesPath;

//check if backend is already running
if (!isBackendSpawned()) {
	console.log(`Spawning backend at ${binPath}`);

	const backendProc = spawn(binPath, [], {
		detached: true,
		shell: true,
		cwd: cwdPath,
		stdio: 'inherit',
	});

	ipcRenderer.send('child-pid', backendProc.pid);

	backendProc.on('error', (err) => {
		console.error(`Backend Error: ${err}`);
	});

	backendProc.on('exit', (code, signal) => {
		console.error(`Backend exited with code ${code} and signal ${signal}`);
	});
} else {
	console.log(`Backend already running, skipping spawn step`);
}

