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
TaskManagementSystem/
│── Controllers/ # API endpoints
│── Data/ # EF Core DbContext
│── Models/ # Entity models & DTOs
│── Repository/ # Repository interfaces & implementations
│── Service/ # Business logic services
│── Utilities/ # Global exception middleware
│── Program.cs # Application entry point
