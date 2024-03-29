import react from '@vitejs/plugin-react';
import {defineConfig} from 'vite';
import viteTsconfigPaths from 'vite-tsconfig-paths';

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [react(), viteTsconfigPaths()],
	server: {
		port: 3000,
		open: '/',
		proxy: {
			'/api': {
				target: 'http://localhost:5219',
				changeOrigin: true,
			},
		},
	},
	build: {
		outDir: 'build',
	},
});
