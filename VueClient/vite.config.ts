import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  build: {
    outDir: '../Host/wwwroot/client',
    emptyOutDir: true,
    sourcemap: true,
    rollupOptions: {
      input: ['./src/main.ts'],
    },
  }
})
