// See the Electron documentation for details on how to use preload scripts:
// https://www.electronjs.org/docs/latest/tutorial/process-model#preload-scripts
import path from 'path';

import os from 'os';

import {spawn} from 'child_process';
import {ipcRenderer} from 'electron';

const platform = os.platform();
let binName = 'RetireSimple.Backend.exe';
if (platform === 'darwin' || platform === 'linux') {
	binName = 'RetireSimple.Backend';
}

const binPath =
	process.env.NODE_ENV === 'development'
		? path.join(__dirname, '..', 'resources', binName)
		: path.join(process.resourcesPath, binName);
const cwdPath =
	process.env.NODE_ENV === 'development'
		? path.join(__dirname, '..', 'resources')
		: process.resourcesPath;

console.log(`Starting backend at ${binPath}`);

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

