# 📚 ManagementBook - ASP.NET Web Application

![.NET](https://img.shields.io/badge/ASP.NET-Core-blue)
![Platform](https://img.shields.io/badge/Platform-WebApp-lightgrey)
![Status](https://img.shields.io/badge/Version-1.0.0-success)
![License](https://img.shields.io/badge/License-MIT-green)

**📍 Developed with ASP.NET Core - April 2025**

An ASP.NET Core web application built to manage products with role-based access for **Admins** and **Employees**.

---

## 🚀 Features Overview

### 🔐 Login Page
- Users can log in as **Admin** or **Employee**.
- Role-based redirection to dashboards.

### 📊 Dashboard (Admin & Employee)
- Only one dashboard page exists, which dynamically adapts based on the user’s role.

### 🛠️ Admin Capabilities
- View all product data, including deleted items.
- Can perform hard and soft delete.
- Restore deleted products regardless of who deleted them.

### 👨‍💼 Employee Capabilities
- Limited to employee functions only.
- Can only perform **soft delete**.
- Soft-deleted products by employees are hidden from their own product list.

---

## 💾 Database Seeding

The system includes data seeding for initial setup and testing.

### 📦 Tables:
- `Users`: Stores Admin and Employee accounts.
- `Products`: Stores product information, with soft delete support.

---

## 🧪 Testing Scenario

### Test Accounts
- **Admin**: Accesses full system controls.
- **Employee**: Accesses limited features.

You can test the login feature and deletion logic using seeded user accounts.

---

## ⚙️ Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server 2022
- Razor Views

---

## 📂 Project Structure

```bash
/ManagementBook
├── /Controllers       # Handles route logic
├── /Views             # Razor view templates
├── /Models            # Data models (Product, User...)
├── /Data              # EF Core context and seed
├── /Repositories      # Data access layer
├── /Services          # Business logic layer
└── appsettings.json   # Connection strings & configs
```

---

## 🙌 Contribution
Feel free to fork, open issues, or submit PRs to improve the project.

---


