# 🛍️ Product Catalog API

A RESTful Web API built with **ASP.NET Core** and **Entity Framework Core** that manages products and categories for an e-commerce backend. This project is part of a portfolio series demonstrating core .NET Web API concepts for junior-level development.

---

## 📌 Project Overview

This API simulates the backend of an e-commerce product catalog. It allows clients to create, read, update and delete products and categories through a clean, well-structured REST interface. The project is intentionally scoped to be small but covers all the foundational concepts expected in a junior .NET developer role.

> **Portfolio Goal:** Demonstrate understanding of REST conventions, Entity Framework Core (Code First), Repository & Service patterns, DTOs, model validation, pagination, soft deletes, dependency injection and global error handling.

---

## 🧰 Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 10 | Web API framework |
| Entity Framework Core | ORM for database access |
| SQL Server / SQLite | Relational database |
| Scalar / OpenAPI | API documentation & testing |
| Data Annotations | Model validation |

---

## ✨ Features

- ✅ Full **CRUD operations** for Products and Categories
- ✅ **Code First** migrations with EF Core
- ✅ **Repository Pattern** for data access abstraction
- ✅ **Service Layer** for business logic separation
- ✅ **DTOs** to decouple API responses from database models
- ✅ **Data Annotations** for input validation
- ✅ **Pagination & Filtering** on product listings
- ✅ **Soft Delete** - products are deactivated, not permanently removed
- ✅ **Global Exception Handling Middleware**
- ✅ **Dependency Injection** throughout
- ✅ **Seed Data** for quick local testing
- ✅ **Scalar UI** for interactive API documentation

---

## 🗂️ Project Structure

```
ProductCatalogAPI/
│
├── Controllers/                  # API endpoints (HTTP layer only)
│   ├── ProductsController.cs
│   └── CategoriesController.cs
│
├── Models/                       # EF Core entity models (database tables)
│   ├── Product.cs
│   └── Category.cs
│
├── DTOs/                         # Data Transfer Objects (request/response shapes)
│   ├── Product/
│   │   ├── CreateProductDto.cs
│   │   ├── UpdateProductDto.cs
│   │   └── ProductResponseDto.cs
│   └── Category/
│       ├── CreateCategoryDto.cs
│       └── CategoryResponseDto.cs
│
├── Data/                         # Database context and seeding
│   ├── AppDbContext.cs
│   └── Seed/
│       └── DataSeeder.cs
│
├── Repositories/                 # Data access layer (Repository pattern)
│   ├── Interfaces/
│   │   ├── IProductRepository.cs
│   │   └── ICategoryRepository.cs
│   ├── ProductRepository.cs
│   └── CategoryRepository.cs
│
├── Services/                     # Business logic layer
│   ├── Interfaces/
│   │   ├── IProductService.cs
│   │   └── ICategoryService.cs
│   ├── ProductService.cs
│   └── CategoryService.cs
│
├── Middleware/                   # Custom middleware pipeline
│   └── ExceptionHandlingMiddleware.cs
│
├── Helpers/                      # Utility classes
│   └── PaginationParams.cs
│
└── Program.cs                    # App entry point, DI registration, middleware setup
```

---

## 🗃️ Data Models

### Category

| Property | Type | Constraints |
|---|---|---|
| Id | int | Primary Key, Auto-increment |
| Name | string | Required, Max 100 chars |
| Description | string | Optional |
| CreatedAt | DateTime | Auto-set on creation |

### Product

| Property | Type | Constraints |
|---|---|---|
| Id | int | Primary Key, Auto-increment |
| Name | string | Required, Max 200 chars |
| Description | string | Optional |
| Price | decimal | Required, Must be > 0 |
| Stock | int | Required, Must be >= 0 |
| SKU | string | Unique product code |
| IsActive | bool | Soft delete flag (default: true) |
| CategoryId | int | Foreign Key → Category |
| CreatedAt | DateTime | Auto-set on creation |
| UpdatedAt | DateTime | Auto-updated on every edit |

---

## 🛣️ API Endpoints

### Categories

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/categories` | Get all categories |
| GET | `/api/categories/{id}` | Get category by ID |
| POST | `/api/categories` | Create a new category |
| PUT | `/api/categories/{id}` | Update a category |
| DELETE | `/api/categories/{id}` | Delete a category |

### Products

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | Get all products (supports pagination & filtering) |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/category/{categoryId}` | Get all products in a category |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Soft delete a product (sets IsActive = false) |

### Query Parameters for `GET /api/products`

| Parameter | Type | Description | Example |
|---|---|---|---|
| `pageNumber` | int | Page number (default: 1) | `?pageNumber=2` |
| `pageSize` | int | Items per page (default: 10) | `?pageSize=5` |
| `search` | string | Filter by product name | `?search=laptop` |
| `categoryId` | int | Filter by category | `?categoryId=3` |

---

## 🏃 Getting Started

### Prerequisites

- .NET 10 SDK
- SQLite
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### 1. Clone the repository

```bash
git clone https://github.com/zaynabid/dotnet-api-showcase.git
cd dotnet-api-showcase/1-products-crud-api
```

### 2. Configure the database connection

Open `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ProductCatalogDb.db"
  }
}
```

### 3. Apply migrations and seed data

```bash
dotnet ef database update
```

This will create the database, apply all migrations and seed it with sample categories and products automatically.

### 4. Run the API

```bash
dotnet run
```

### 5. Open Scalar UI

Navigate to:

```
https://localhost:{port}/scalar
```

You'll see the full interactive API documentation where you can test every endpoint directly from the browser.

---

## 🧠 Key Concepts Demonstrated

### Repository Pattern
The repository pattern abstracts all database queries behind interfaces (`IProductRepository`, `ICategoryRepository`). This means the controllers never talk to the database directly - they go through the service layer, which talks to repositories. This makes the code easier to test and maintain.

### Service Layer
Business logic lives in the service layer, not in controllers. For example, checking whether a category exists before creating a product happens in `ProductService`, not in `ProductsController`. Controllers are kept thin - they only handle HTTP concerns.

### DTOs (Data Transfer Objects)
The API never exposes EF Core models directly. Instead, it uses separate DTOs for requests (`CreateProductDto`) and responses (`ProductResponseDto`). This protects the database schema from being leaked in API responses and gives full control over what data is sent and received.

### Soft Delete
Deleting a product sets `IsActive = false` rather than removing the row from the database. This is a common real-world pattern that preserves data integrity and allows recovery of deleted records. All `GET` queries automatically filter out inactive products.

### Global Exception Middleware
All unhandled exceptions are caught by `ExceptionHandlingMiddleware` and returned as clean, consistent JSON error responses - instead of raw stack traces being exposed to clients.

### Pagination
The `GET /api/products` endpoint returns paginated results using `PaginationParams` (pageNumber + pageSize). This is essential for any production API that deals with large datasets.

---

## 📋 Sample Request & Response

### Create a Product

**Request**
```http
POST /api/products
Content-Type: application/json

{
  "name": "Mechanical Keyboard",
  "description": "TKL layout, Cherry MX Red switches",
  "price": 89.99,
  "stock": 50,
  "sku": "KB-MX-TKL-001",
  "categoryId": 2
}
```

**Response** `201 Created`
```json
{
  "id": 12,
  "name": "Mechanical Keyboard",
  "description": "TKL layout, Cherry MX Red switches",
  "price": 89.99,
  "stock": 50,
  "sku": "KB-MX-TKL-001",
  "isActive": true,
  "categoryId": 2,
  "categoryName": "Peripherals",
  "createdAt": "2025-03-14T10:30:00Z"
}
```

---

## 🚫 Out of Scope (Intentional)

The following were deliberately excluded to keep the project focused:

- Authentication & Authorization → covered in **Project 2 (Auth API)**
- Caching & Rate Limiting → covered in **Project 3 (Middleware API)**
- File uploads
- Frontend / UI

---

## 📂 Part of the .NET API Showcase Series

| Project | Concepts |
|---|---|
| **1. Product Catalog API** ← you are here | CRUD, EF Core, Repository, DTOs, Pagination |
| 2. Auth API *(coming soon)* | JWT, Refresh Tokens, ASP.NET Identity, Roles |
| 3. Middleware API *(coming soon)* | Custom Middleware, Caching, Rate Limiting, API Versioning |

---

## 👤 Author

Built by **Zayn** as part of a .NET Web API portfolio.  
Feel free to connect on [LinkedIn](https://linkedin.com/in/zaynabid) or check out my other projects on [GitHub](https://github.com/zaynabid).
