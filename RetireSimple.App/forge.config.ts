import {MakerZIP} from '@electron-forge/maker-zip';
import {AutoUnpackNativesPlugin} from '@electron-forge/plugin-auto-unpack-natives';
import {WebpackPlugin} from '@electron-forge/plugin-webpack';
import type {ForgeConfig} from '@electron-forge/shared-types';

import {mainConfig} from './webpack.main.config';
import {rendererConfig} from './webpack.renderer.config';

const config: ForgeConfig = {
	packagerConfig: {
		asar: true,
		extraResource: ['./backend/'],
		win32metadata: {
			CompanyName: 'RetireSimple Team',
			FileDescription: 'RetireSimple',
			OriginalFilename: 'RetireSimple.exe',
			ProductName: 'RetireSimple',
			InternalName: 'RetireSimple',
		},
		icon: './backend/wwwroot/favicon.ico',
	},
	rebuildConfig: {},
	makers: [new MakerZIP({}, ['win32', 'darwin', 'linux'])],
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

