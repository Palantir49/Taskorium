import {defineConfig} from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [react()],
    server: {
        port: 5000,
        open: true,
        proxy: {
            '/api': {
                target: 'http://localhost:5050',
                changeOrigin: true,
                secure: false,
            },
            '/api/v1': {
                target: 'http://localhost:5050',
                changeOrigin: true,
                secure: false,
            }
        }
    }
})


