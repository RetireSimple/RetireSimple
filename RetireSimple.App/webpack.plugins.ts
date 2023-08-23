import type IForkTsCheckerWebpackPlugin from 'fork-ts-checker-webpack-plugin';
// eslint-disable-next-line import/default
import CopyPlugin from 'copy-webpack-plugin';
// eslint-disable-next-line @typescript-eslint/no-var-requires
const noop = require('noop-webpack-plugin');

// eslint-disable-next-line @typescript-eslint/no-var-requires
const ForkTsCheckerWebpackPlugin: typeof IForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');

export const plugins = [
	new ForkTsCheckerWebpackPlugin({
		logger: 'webpack-infrastructure',
	}),
	process.env.NODE_ENV === 'production'
		? new noop()
		: new CopyPlugin({
				patterns: [
					{
						from: 'backend/**',
						to: '.',
						globOptions: {dot: true, ignore: ['**/node_modules/**']},
					},
				],
		}),
];

