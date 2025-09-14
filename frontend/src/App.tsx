import React, { useEffect, useMemo, useState } from 'react'
import { DragDropContext, Droppable, Draggable, DropResult } from '@hello-pangea/dnd'
import { createTask, deleteTask, listTasks, updateTask } from './api'
import type { Task, TaskStatus } from './types'

const STATUS_LABEL: Record<TaskStatus, string> = { 0: 'To Do', 1: 'In Progress', 2: 'Done' }

const emptyDraft = { title: '', description: '', status: 0 as TaskStatus }

export default function App() {
  const [tasks, setTasks] = useState<Task[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [isModalOpen, setModalOpen] = useState(false)
  const [draft, setDraft] = useState<{ id?: string; title: string; description?: string; status: TaskStatus }>(emptyDraft)

  useEffect(() => {
    (async () => {
      try {
        setLoading(true)
        const data = await listTasks()
        setTasks(data)
      } catch (e: any) {
        setError(e?.message ?? 'Failed to fetch')
      } finally {
        setLoading(false)
      }
    })()
  }, [])

  const byStatus = useMemo(() => {
    const map: Record<TaskStatus, Task[]> = { 0: [], 1: [], 2: [] }
    tasks.forEach(t => map[t.status].push(t))
    return map
  }, [tasks])

  async function onDragEnd(result: DropResult) {
    const { source, destination, draggableId } = result
    if (!destination) return
    const srcStatus = Number(source.droppableId) as TaskStatus
    const dstStatus = Number(destination.droppableId) as TaskStatus
    if (srcStatus === dstStatus) return

    const item = tasks.find(t => t.id === draggableId)
    if (!item) return

    // optimistic update
    setTasks(prev => prev.map(t => t.id === item.id ? { ...t, status: dstStatus } : t))
    try {
      await updateTask(item.id, { title: item.title, description: item.description ?? null, status: dstStatus })
    } catch (e) {
      // rollback
      setTasks(prev => prev.map(t => t.id === item.id ? { ...t, status: srcStatus } : t))
      alert('Failed to move task')
    }
  }

  function openCreate(status: TaskStatus) {
    setDraft({ ...emptyDraft, status })
    setModalOpen(true)
  }

  function openEdit(task: Task) {
    setDraft({ id: task.id, title: task.title, description: task.description ?? '', status: task.status })
    setModalOpen(true)
  }

  async function submitDraft() {
    try {
      if (!draft.title.trim()) {
        alert('Title is required')
        return
      }
      if (draft.id) {
        const updated = await updateTask(draft.id, { title: draft.title, description: draft.description ?? null, status: draft.status })
        setTasks(prev => prev.map(t => t.id === updated.id ? updated : t))
      } else {
        const created = await createTask({ title: draft.title, description: draft.description ?? null, status: draft.status })
        setTasks(prev => [...prev, created])
      }
      setModalOpen(false)
    } catch {
      alert('Save failed')
    }
  }

  async function remove(id: string) {
    if (!confirm('Delete this task?')) return
    const backup = tasks
    setTasks(prev => prev.filter(t => t.id !== id))
    try {
      await deleteTask(id)
    } catch {
      setTasks(backup)
      alert('Delete failed')
    }
  }

  if (loading) return <div className="container">Loading…</div>
  if (error) return <div className="container">Error: {error}</div>

  return (
    <div className="container">
      <h2>Kanban Task Board</h2>
      <p style={{opacity:.7}}>API: {import.meta.env.VITE_API_BASE || 'http://localhost:5121'}</p>

      <DragDropContext onDragEnd={onDragEnd}>
        <div className="board">
          {[0,1,2].map((status) => (
            <Column
              key={status}
              status={status as TaskStatus}
              title={STATUS_LABEL[status as TaskStatus]}
              items={byStatus[status as TaskStatus]}
              onAdd={() => openCreate(status as TaskStatus)}
              onEdit={openEdit}
              onDelete={remove}
            />
          ))}
        </div>

        {isModalOpen && (
          <div className="modal" onClick={() => setModalOpen(false)}>
            <div className="box" onClick={e => e.stopPropagation()}>
              <h3>{draft.id ? 'Edit Task' : 'New Task'}</h3>
              <div className="row">
                <select className="select" value={draft.status} onChange={e => setDraft(s => ({...s, status: Number(e.target.value) as TaskStatus}))}>
                  <option value={0}>To Do</option>
                  <option value={1}>In Progress</option>
                  <option value={2}>Done</option>
                </select>
                <button className="btn" onClick={() => setModalOpen(false)}>Close</button>
              </div>
              <div style={{marginTop:8}}>
                <input className="input" placeholder="Title" value={draft.title} onChange={e => setDraft(s => ({...s, title: e.target.value}))}/>
              </div>
              <div style={{marginTop:8}}>
                <textarea className="textarea" rows={4} placeholder="Description (optional)" value={draft.description} onChange={e => setDraft(s => ({...s, description: e.target.value}))}/>
              </div>
              <div className="row" style={{marginTop:12, justifyContent:'flex-end'}}>
                <button className="btn primary" onClick={submitDraft}>{draft.id ? 'Save' : 'Create'}</button>
              </div>
            </div>
          </div>
        )}
      </DragDropContext>
    </div>
  )
}

function Column({
  status, title, items, onAdd, onEdit, onDelete
}: {
  status: TaskStatus
  title: string
  items: Task[]
  onAdd: () => void
  onEdit: (t: Task) => void
  onDelete: (id: string) => void
}) {
  return (
    <div className="column">
      <div className="row" style={{justifyContent:'space-between', alignItems:'center'}}>
        <div className="col-title">{title}</div>
        <button className="btn" onClick={onAdd}>+ Add</button>
      </div>

      <Droppable droppableId={String(status)}>
        {(provided) => (
          <div ref={provided.innerRef} {...provided.droppableProps}>
            {items.map((t, idx) => (
              <Draggable key={t.id} draggableId={t.id} index={idx}>
                {(drag) => (
                  <div
                    className="card"
                    ref={drag.innerRef}
                    {...drag.draggableProps}
                    {...drag.dragHandleProps}
                    onDoubleClick={() => onEdit(t)}
                  >
                    <div style={{fontWeight:600}}>{t.title}</div>
                    {t.description ? <div style={{opacity:.8, marginTop:4, whiteSpace:'pre-wrap'}}>{t.description}</div> : null}
                    <div className="row" style={{marginTop:8, justifyContent:'flex-end'}}>
                      <button className="btn" onClick={() => onEdit(t)}>Edit</button>
                      <button className="btn danger" onClick={() => onDelete(t.id)}>Delete</button>
                    </div>
                  </div>
                )}
              </Draggable>
            ))}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    </div>
  )
}
