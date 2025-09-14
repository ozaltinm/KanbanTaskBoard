import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// VITE_API_BASE env ile geldiğinde kullanırız
export default defineConfig({
  plugins: [react()],
})
