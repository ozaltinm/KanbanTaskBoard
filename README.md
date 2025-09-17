# Kanban Task Board

A simple **Kanban board application** built with:

- **Backend**: ASP.NET Core 9.0 (REST API + EF Core + FluentValidation)
- **Frontend**: React (Vite)
- **Database**: SQLite (mounted as a volume in `./data`)
- **Containerization**: Docker Compose

---

## 🚀 Features

- Create, update, delete, and list tasks
- Tasks have:
  - Title
  - Description
  - Status (`ToDo`, `InProgress`, `Done`)
  - Created / Updated timestamps
- REST API with Swagger UI
- CORS enabled for local frontend (`http://localhost:5173`)
- Persistent data via SQLite in `./data/kanban.db`

---

## 🛠️ Prerequisites

- [Docker & Docker Compose](https://docs.docker.com/get-docker/)
- [.NET 9 SDK](https://dotnet.microsoft.com/) (for local dev without Docker)
- [Node.js](https://nodejs.org/) (for frontend dev mode)

---

## ▶️ Run with Docker Compose

From project root (`kanban/`):

```bash
docker compose build --no-cache
docker compose up -d
```

### Services

- **Backend API** → [http://localhost:5121](http://localhost:5121)  
  Swagger UI: [http://localhost:5121/swagger](http://localhost:5121/swagger)

- **Frontend** → [http://localhost:5173](http://localhost:5173)

### Logs

```bash
docker compose logs -f api
docker compose logs -f web
```

### Stop containers

```bash
docker compose down
```

---

## 🧑‍💻 Local Development

### Backend (ASP.NET Core)

```bash
cd backend
dotnet restore
dotnet run
```

- Runs on `http://localhost:5121`
- SQLite file: `../data/kanban.db`
- Entity Framework Core migrations:

```bash
dotnet ef migrations add Init
dotnet ef database update
```

### Frontend (React + Vite)

```bash
cd frontend
npm install
npm run dev
```

- Runs on `http://localhost:5173`
- Configured to call backend API via `VITE_API_BASE` env var.

---

## 📂 Project Structure

```
kanban/
├── backend/              # ASP.NET Core API
│   ├── Controllers/      # API endpoints
│   ├── Domain/           # Entities
│   ├── Infrastructure/   # EF Core DbContext
│   ├── Application/      # Services & DTOs
│   └── Middleware/       # Exception handling
│
├── frontend/             # React (Vite)
│   ├── src/              # React components
│   └── public/           # Static assets
│
├── data/                 # SQLite DB volume
├── docker-compose.yml
├── README.md
└── .gitignore
```

---

## ✅ Todo

- [ ] Add authentication & user management  
- [ ] Deploy to cloud (Azure / AWS / GCP)  
- [ ] Add unit & integration tests  
