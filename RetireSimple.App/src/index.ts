import {app, BrowserWindow, ipcMain} from 'electron';
import * as ps from 'ps-node';

declare const MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY: string;
declare const MAIN_WINDOW_WEBPACK_ENTRY: string;

const child_pid: number[] = [];

// Handle creating/removing shortcuts on Windows when installing/uninstalling.
if (require('electron-squirrel-startup')) {
	app.quit();
}

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

	// Open the DevTools.
	if (process.env.NODE_ENV === 'development') mainWindow.webContents.openDevTools();
};

//Keep track of child processes
ipcMain.on('child-pid', (event, arg) => {
	console.log(`Child process ${arg} started`);
	child_pid.push(arg);
});

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
	child_pid.forEach((pid) => {
		console.log(`Killing child process ${pid}`);
		ps.kill(pid, (err) => {
			if (err) {
				throw new Error(err.message);
			}
		});
	});
});

