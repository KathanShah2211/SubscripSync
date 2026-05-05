# SubscripSync 🔄

**SubscripSync** is a modern, full-stack subscription management platform designed to help users track recurring expenses, visualize spending habits, and stay on top of renewals — all from a sleek dark-mode dashboard.

Built with **.NET 9 (Web API)** and **Angular 19**, following **Clean Architecture** principles with CQRS pattern.

---

## ✨ Preview

> Premium dark-mode SaaS dashboard with glassmorphism, animated backgrounds, and real-time notifications.

---

## 🚀 Key Features

- **📊 Interactive Dashboard** — View all active subscriptions with category-colored cards, monthly cost summary, and next renewal dates
- **📈 Analytics** — Doughnut & bar charts showing spending distribution by category with dark-themed visualizations
- **👤 Profile Management** — Update personal info and preferred currency settings
- **🔔 Real-Time Notifications** — SignalR-powered alerts for upcoming renewals and payment confirmations
- **🔐 JWT Authentication** — Secure user registration & login with token-based auth
- **🔄 Auto-Renewal Engine** — Background worker service that automatically calculates next billing dates (Weekly / Monthly / Yearly)
- **🐳 Docker Ready** — One-command deployment with Docker Compose (API + Client + SQL Server)

---

## 🛠️ Tech Stack

### Backend
| Component | Technology |
|-----------|-----------|
| Framework | .NET 9 Web API |
| Architecture | Clean Architecture (Domain → Application → Infrastructure → API) |
| Database | SQL Server (EF Core Code-First) |
| Patterns | CQRS (MediatR), Repository Pattern, Dependency Injection |
| Auth | JWT Bearer Tokens |
| Real-Time | SignalR |
| Docs | Swagger / OpenAPI |

### Frontend
| Component | Technology |
|-----------|-----------|
| Framework | Angular 19 (Standalone Components) |
| Styling | Tailwind CSS + Angular Material (Custom Dark Theme) |
| Charts | ng2-charts (Chart.js) |
| Alerts | SweetAlert2 (Dark Themed) |
| State | RxJS |

### Infrastructure
| Component | Technology |
|-----------|-----------|
| Containerization | Docker + Docker Compose |
| Web Server | Nginx (SPA Routing) |
| CI | GitHub Actions |

---

## ⚙️ Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (recommended)
- Or: [.NET 9 SDK](https://dotnet.microsoft.com/download) + [Node.js v20+](https://nodejs.org/) + SQL Server

### 🐳 Quick Start (Docker)

1. **Clone the repository**
   ```bash
   git clone https://github.com/KathanShah2211/SubscripSync.git
   cd SubscripSync
   ```

2. **Create environment file**
   ```bash
   # Create .env in the project root with:
   MSSQL_PASSWORD=YourStrong@Passw0rd
   JWT_SECRET=your-super-secret-key-at-least-32-characters-long
   ```

3. **Start everything**
   ```bash
   docker-compose up -d --build
   ```

4. **Access the app**
   - 🌐 **Frontend**: [http://localhost:4200](http://localhost:4200)
   - 🔧 **API / Swagger**: [http://localhost:5083/swagger](http://localhost:5083/swagger)

> **Note:** If you see database migration errors, run `docker-compose down -v` then `docker-compose up -d --build` to start fresh.

### 💻 Local Development (Without Docker)

1. **Backend**
   ```bash
   cd SubscripSync.API
   dotnet restore
   dotnet run
   ```
   *API starts at `http://localhost:5083`*

2. **Frontend**
   ```bash
   cd SubscripSync.Client
   npm install
   npm start
   ```
   *Client starts at `http://localhost:4200`*

---

## 🧪 Running Tests

```bash
dotnet test
```

Unit tests are implemented using **xUnit** to verify business logic (subscription renewal calculations, etc.).

---

## 📂 Project Structure

```
SubscripSync/
├── SubscripSync.API/              # Entry point — Controllers, Program.cs
├── SubscripSync.Application/      # Business logic — CQRS Commands/Queries (MediatR)
├── SubscripSync.Domain/           # Core — Entities, Interfaces
├── SubscripSync.Infrastructure/   # Implementation — EF Core, Repositories, Auth, Workers
├── SubscripSync.Client/           # Angular 19 frontend (dark theme UI)
├── SubscripSync.Tests/            # Unit tests (xUnit)
├── .github/workflows/             # CI/CD pipeline
├── docker-compose.yml             # Container orchestration
└── .env                           # Secrets (NOT committed — see .gitignore)
```

---

## 🎨 UI Design System

The frontend uses a custom **dark-mode design system** built on top of Angular Material + Tailwind CSS:

- **Theme**: Deep dark background (`#0c0a14`) with glassmorphic cards
- **Font**: Inter (Google Fonts)
- **Accent**: Purple/Indigo gradient (`#8c3fff → #6d1bf0`)
- **Effects**: Backdrop blur, hover lifts, staggered entrance animations
- **Category Colors**: Each subscription category has a distinct color (Entertainment → Fuchsia, Software → Blue, etc.)

---

## 🔒 Security

- JWT secrets and database passwords are stored in `.env` (excluded from version control)
- `appsettings.json` contains only placeholder values — real secrets are injected via environment variables in Docker
- Passwords are hashed using BCrypt

---

## 🤝 Contributing

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
