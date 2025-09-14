import axios from 'axios'
import type { Task } from './types'

// .env yoksa default localhost:5121
const baseURL = import.meta.env.VITE_API_BASE || 'http://localhost:5121'
export const api = axios.create({ baseURL })

export async function listTasks(): Promise<Task[]> {
  const { data } = await api.get('/api/tasks')
  return data
}

export async function createTask(payload: { title: string; description?: string | null; status?: number }): Promise<Task> {
  const { data } = await api.post('/api/tasks', payload)
  return data
}

export async function updateTask(id: string, payload: { title: string; description?: string | null; status: number }): Promise<Task> {
  const { data } = await api.put(`/api/tasks/${id}`, payload)
  return data
}

export async function deleteTask(id: string): Promise<void> {
  await api.delete(`/api/tasks/${id}`)
}
