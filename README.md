# SubscripSync 🔄

**SubscripSync** is a modern, full-stack subscription management platform designed to help users track recurring expenses, visualize spending habits, and avoid "subscription fatigue."

Built with **.NET 9 (Web API)** and **Angular 19**, it follows **Clean Architecture** principles to demonstrate enterprise-grade software design.

![Dashboard Preview](https://via.placeholder.com/800x400?text=SubscripSync+Dashboard+Preview)

## 🚀 Key Features

- **📊 Interactive Dashboard**: View all active subscriptions, costs, and next payment dates at a glance.
- **📈 Analytics Module**: Visualize spending distribution by category (Entertainment, Software, etc.) and monitor monthly run rates.
- **🔄 Renewal Engine**: Background services (HostedService) automatically calculate next billing dates based on custom cycles (Weekly, Monthly, Yearly).
- **🛡️ Secure & Scalable**: Implementation of Clean Architecture with CQRS pattern (MediatR) and Entity Framework Core.
- **⚡ Modern UI**: sleek, responsive interface built with Angular Material and Tailwind CSS.

## 🛠️ Tech Stack

### Backend
- **Framework**: .NET 9 (Core)
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API)
- **Database**: SQL Server (EF Core Code-First)
- **Patterns**: CQRS (MediatR), Repository Pattern, Dependency Injection
- **Documentation**: Swagger UI / OpenAPI

### Frontend
- **Framework**: Angular 19 (Standalone Components)
- **Styling**: Tailwind CSS + Angular Material
- **Charts**: ng2-charts (Chart.js wrapper)
- **State**: RxJS Reactive Extensions

## ⚙️ Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v20+)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB or Docker)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/yourusername/SubscripSync.git
    cd SubscripSync
    ```

2.  **Setup Backend**
    ```bash
    cd SubscripSync.API
    dotnet restore
    dotnet run
    ```
    *The API will start at `http://localhost:5083`. Swagger UI is available at `http://localhost:5083/swagger`.*

3.  **Setup Frontend**
    ```bash
    cd ../SubscripSync.Client
    npm install
    npm start
    ```
    *The Client will start at `http://localhost:4200`.*

## 🧪 Running Tests

Unit tests are implemented using **xUnit** to verify business logic (e.g., subscription renewal calculations).

```bash
dotnet test
```

## 📂 Project Structure

```
SubscripSync/
├── SubscripSync.API/           # Entry point, Controllers
├── SubscripSync.Application/   # Business Logic, CQRS Handlers
├── SubscripSync.Domain/        # Entities, Interfaces (Core)
├── SubscripSync.Infrastructure/# DB Context, Repositories, Ext Services
├── SubscripSync.Client/        # Angular 19 Frontend
└── SubscripSync.Tests/         # Unit Tests
```

## 🤝 Contributing

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
