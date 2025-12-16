
# E-Commerce Web API

## 📌 Project Overview

The **E-Commerce Web API** is a fully functional backend system built using **ASP.NET Core Web API** following **Clean Architecture** principles. The project provides all core e-commerce features including authentication, product management, shopping basket, order processing, and payment integration.

The system is designed with **performance, scalability, and security** in mind, utilizing **JWT authentication**, **Redis caching**, and **Entity Framework Core** with **SQL Server**.

---

## 🛠️ Technologies & Tools

* **ASP.NET Core Web API**
* **C#**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Core Identity**
* **JWT Authentication**
* **Redis Cache**
* **Clean Architecture**
* **Swagger (OpenAPI)**

---

## ✨ Key Features

### 🔐 Authentication & Authorization

* Secure user registration & login
* JWT-based authentication
* Refresh Token support
* Role-based access control (Admin / User)
* ASP.NET Core Identity integration

---

### 🛍️ Shopping Basket (Cart)

* High-performance basket system stored in **Redis**
* Full CRUD operations on basket items
* Automatic price and quantity synchronization
* Reduced database load using in-memory caching

---

### 📦 Orders & Payments

* Order creation and management
* Order status tracking
* Online payment workflow integration
* Secure checkout process

---

### ⚡ Performance Optimization

* Redis caching for frequently accessed data
* Optimized database queries
* Reduced response time and server load

---

## 🧩 Clean Architecture Structure

```
E-Commerce
│
├── Core
│   ├── Entities
│   ├── Interfaces
│   └── Specifications
│
├── Application
│   ├── DTOs
│   ├── Services
│   └── Mappings
│
├── Infrastructure
│   ├── Data
│   ├── Identity
│   └── Repositories
│
├── API
│   ├── Controllers
│   ├── Middleware
│   └── Extensions
```

---

## 🚀 How to Run the Project

1. Clone the repository

   ```bash
   git clone https://github.com/your-username/E-Commerce-Web-API.git
   ```

2. Open the solution in **Visual Studio**

3. Configure connection strings in `appsettings.json`

   * SQL Server
   * Redis

4. Apply database migrations

   ```bash
   Update-Database
   ```

5. Run the project

   ```bash
   Ctrl + F5
   ```

6. Open Swagger

   ```
   https://localhost:{port}/swagger
   ```

---

## 📂 Database

* SQL Server
* Entity Framework Core (Code First)
* ASP.NET Core Identity Tables

---

## 📌 API Documentation

* Swagger UI for testing endpoints
* Organized controllers and endpoints

---

## 🧠 Learning Outcomes

* Building scalable Web APIs
* Implementing Clean Architecture
* JWT & Identity security
* Redis caching strategies
* Performance optimization techniques

---

## 👨‍💻 Author

**Omar Mohamed**

---

## 📄 License

This project is developed for learning and portfolio purposes.
