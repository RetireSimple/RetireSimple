import type IForkTsCheckerWebpackPlugin from 'fork-ts-checker-webpack-plugin';
// eslint-disable-next-line import/default
import CopyPlugin from 'copy-webpack-plugin';

// eslint-disable-next-line @typescript-eslint/no-var-requires
const ForkTsCheckerWebpackPlugin: typeof IForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');

export const plugins = [
	new ForkTsCheckerWebpackPlugin({
		logger: 'webpack-infrastructure',
	}),
	new CopyPlugin({
		patterns: [
			{
				from: 'resources/**',
				to: '.',
				globOptions: {dot: true, ignore: ['**/node_modules/**']},
			},
		], // eslint-disable-line @typescript-eslint/no-unsafe-member-access
	}),
];

