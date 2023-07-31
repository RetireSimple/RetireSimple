// See the Electron documentation for details on how to use preload scripts:
// https://www.electronjs.org/docs/latest/tutorial/process-model#preload-scripts
import path from 'path';

import os from 'os';

import * as process from 'child_process';

const platform = os.platform();
let binName = 'RetireSimple.Backend.exe';
if (platform === 'darwin') {
	binName = 'RetireSimple.Backend';
}

const binPath = path.join(__dirname, '..', 'resources', binName);
const cwdPath = path.join(__dirname, '..', 'resources');

console.log(`Starting backend at ${binPath}`);

const backendProc = process.spawn(binPath, [], {
	detached: true,
	shell: true,
	cwd: cwdPath,
	stdio: 'inherit',
});

backendProc.on('error', (err) => {
	console.error(`Backend Error: ${err}`);
});

backendProc.on('exit', (code, signal) => {
	console.error(`Backend exited with code ${code} and signal ${signal}`);
});

// process..on('exit', () => {
// 	backendProc.kill();
// });
