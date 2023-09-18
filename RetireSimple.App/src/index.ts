import {exec, spawn, ChildProcess} from 'child_process';
import {app, BrowserWindow} from 'electron';
import os from 'os';
import path from 'path';
declare const MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY: string;
declare const MAIN_WINDOW_WEBPACK_ENTRY: string;

// Handle creating/removing shortcuts on Windows when installing/uninstalling.
if (require('electron-squirrel-startup')) {
	app.quit();
}

const child_procs: ChildProcess[] = [];

const spawnBackend = () => {
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
			callback(stdout.toLowerCase().indexOf('retiresimple.backend') > -1);
		});
		return exec_result;
	};

	const binName =
		platform === 'darwin' || platform === 'linux'
			? 'RetireSimple.Backend'
			: 'RetireSimple.Backend.exe';

	const binPath =
		process.env.NODE_ENV === 'development'
			? path.join(__dirname, '..', '..', 'backend', binName)
			: path.join(process.resourcesPath, 'backend', binName);
	const cwdPath =
		process.env.NODE_ENV === 'development'
			? path.join(__dirname, '..', '..', 'backend')
			: path.join(process.resourcesPath, 'backend');

	//check if backend is already running
	if (!isBackendSpawned()) {
		console.log(`Spawning backend at ${binPath}`);

		const backendProc = spawn(binPath, [], {
			detached: true,
			// shell: true, // uncomment if you need backend logs
			cwd: cwdPath,
			stdio: 'inherit',
		});

		child_procs.push(backendProc);

		backendProc.on('error', (err) => {
			console.error(`Backend Error: ${err}`);
		});

		backendProc.on('exit', (code, signal) => {
			console.error(`Backend exited with code ${code} and signal ${signal}`);
		});
	} else {
		console.log(`Backend already running, skipping spawn step`);
	}
};

//check if we have a port
const createWindow = (): void => {
	// Create the browser window.
	const mainWindow = new BrowserWindow({
		height: 600,
		width: 800,
		webPreferences: {
			preload: MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY,
			nodeIntegration: true,
		},
	});

	// and load the index.html of the app.
	mainWindow.loadURL(MAIN_WINDOW_WEBPACK_ENTRY);

	spawnBackend();

	const reload_interval = setInterval(() => {
		fetch('http://localhost:5000/api/Heartbeat').then((res) => {
			if (res.status === 200) {
				clearInterval(reload_interval);
				mainWindow.loadURL('http://localhost:5000/index.html');
			}
		});
	}, 3000);
	// Open the DevTools.
	if (process.env.NODE_ENV === 'development') mainWindow.webContents.openDevTools();
};

app.on('ready', createWindow);

app.on('window-all-closed', () => {
	if (process.platform !== 'darwin') {
		app.quit();
	}
});

app.on('activate', () => {
	// On OS X it's common to re-create a window in the app when the
	// dock icon is clicked and there are no other windows open.
	if (BrowserWindow.getAllWindows().length === 0) {
		createWindow();
	}
});

app.on('before-quit', () => {
	child_procs.forEach((proc) => {
		console.log(`Killing process ${proc.pid}`);
		os.platform() === 'win32' ? proc.kill('SIGTERM') : proc.kill('SIGINT');
		//send ctrl+c, which is SIGINT on linux/mac and SIGTERM on windows
	});
});
