import { fileURLToPath, URL } from 'node:url'
import { readFileSync } from 'fs';
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    https: {
      key: readFileSync('./certs/localhost-key.pem'),
      cert: readFileSync('./certs/localhost.pem'),
    },
    port: 5173
  },
  base: '/'
})
