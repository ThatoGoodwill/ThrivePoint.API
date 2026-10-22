import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'https://localhost:7001', // <--- CHANGE THIS to your actual .NET port
        changeOrigin: true,
        secure: false, // Important for local dev with self-signed certs
      }
    }
  }
})