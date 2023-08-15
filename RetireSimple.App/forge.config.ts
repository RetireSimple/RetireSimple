import type {ForgeConfig} from '@electron-forge/shared-types';
import {MakerSquirrel} from '@electron-forge/maker-squirrel';
import {MakerZIP} from '@electron-forge/maker-zip';
import {MakerDeb} from '@electron-forge/maker-deb';
import {MakerRpm} from '@electron-forge/maker-rpm';
import {AutoUnpackNativesPlugin} from '@electron-forge/plugin-auto-unpack-natives';
import {WebpackPlugin} from '@electron-forge/plugin-webpack';

import {mainConfig} from './webpack.main.config';
import {rendererConfig} from './webpack.renderer.config';

import path from 'path';
import fs from 'fs';

//Used to get all files in the resources folder
const getResourceFileList = (): string[] => {
	const files: string[] = [];
	const resourcePath = path.join(__dirname, 'resources');
	const walkSync = (dir: string, fileList: string[] = []) => {
		fs.readdirSync(dir).forEach((file: string) => {
			const filePath = path.join(dir, file);
			const fileStat = fs.statSync(filePath);

			if (fileStat.isDirectory()) {
				walkSync(filePath, fileList);
			} else {
				fileList.push(filePath);
			}
		});
	};

	walkSync(resourcePath, files);

	return files;
};

const config: ForgeConfig = {
	packagerConfig: {
		asar: true,
		extraResource: getResourceFileList(),
		win32metadata: {
			CompanyName: 'RetireSimple Team',
			FileDescription: 'RetireSimple',
			OriginalFilename: 'RetireSimple.exe',
			ProductName: 'RetireSimple',
			InternalName: 'RetireSimple',
		},
		icon: './resources/wwwroot/favicon.ico',
	},
	rebuildConfig: {},
	makers: [
		new MakerSquirrel({}),
		new MakerZIP({}, ['darwin']),
		new MakerRpm({}),
		new MakerDeb({}),
	],
	plugins: [
		new AutoUnpackNativesPlugin({}),
		new WebpackPlugin({
			mainConfig,
			renderer: {
				config: rendererConfig,
				entryPoints: [
					{
						html: './src/index.html',
						js: './src/renderer.ts',
						name: 'main_window',
						preload: {
							js: './src/preload.ts',
						},
					},
				],
			},
		}),
	],
};

export default config;

