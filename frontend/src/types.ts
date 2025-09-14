export type TaskStatus = 0 | 1 | 2  // ToDo = 0, InProgress = 1, Done = 2

export interface Task {
  id: string
  title: string
  description?: string | null
  status: TaskStatus
  createdAt: string
  updatedAt: string
}
