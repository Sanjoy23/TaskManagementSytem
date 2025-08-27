# Task & Team Management System (ASP.NET Core Web API)

A backend API for managing tasks and teams, built with **.NET Core**, **Entity Framework Core**, **JWT authentication**, **Serilog logging**, and **global exception middleware**.  
This system provides secure endpoints for creating and managing users, teams, and tasks with role-based access control.

---

## 🚀 Features
- **User Authentication & Authorization** (JWT Bearer Token)
- **Role-Based Access Control** (`Admin`, `Manager`, `Employee`)
- **Task Management**
  - Create, assign, update, and delete tasks
  - Status updates restricted to `Employee` role
- **Team Management**
  - Create teams and assign members
- **User management**
  - Create, Update, Read and Delete User
- **Entity Framework Core** with SQL Server
- **Repository & Service Layer Pattern**
- **Global Exception Handling Middleware**
- **Request Validation** using FluentValidation
- **Serilog** for structured logging
- **Swagger/OpenAPI** integration with JWT authentication support

---

## 🛠️ Tech Stack
- **.NET 8 Web API**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **FluentValidation**
- **Serilog**
- **Swagger / Swashbuckle**

---

## 📦 Project Structure
```
TaskManagementSystem/
│── Controllers   →  API endpoints
│── Data          →  EF Core DbContext
│── Models        →  Entity models & DTOs
│── Repository    →  Repository interfaces & implementations
│── Service       →  Business logic services
│── Utilities     →  Global exception middleware
│── Program.cs    →  Application entry point
```


## 🔧 Setup Instructions
1. Prerequisites
- Make sure you have the following installed:
- .NET 8 SDK
- SQL Server (or Docker version)
- Visual Studio 2022 / VS Code
- Postman or Swagger UI for testing APIs

2. Clone the Repository
   - git clone https://github.com/Sanjoy23/TaskManagementSytem.git
   - cd TaskManagementSytem
3. Configure Database Connection
  - Open appsettings.json (or appsettings.Development.json).
  - Find the ConnectionStrings section:
    ```
      "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TaskManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
      }
    ```
   - Replace YOUR_SERVER_NAME with your SQL Server instance name.
4. Apply Database Migrations
  - Run these commands inside the project folder:
    ```
    dotnet restore
    dotnet ef database update
    ```
5. Run the Application
    - dotnet run
   - The API should start at:
      - https://localhost:5001
      - http://localhost:5000
6. Access Swagger UI
    - Open in browser:
    - https://localhost:5001/swagger
   
## 🔑 Authentication & Roles

- First, register a new user using /api/auth/register.
- Then, login using /api/auth/login to get a JWT token.
- Copy the token, click Authorize in Swagger, and paste it as:
- Bearer <your_token_here>
- Access role-based endpoints depending on your user role (Admin, Manager, Employee).

## 📜 Logs
- Logs are written using Serilog.
- By default, they may appear in the console or in a file (check appsettings.json logging section).
                



