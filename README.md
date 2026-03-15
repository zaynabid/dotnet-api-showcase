# .NET Web API Showcase

This repository is a collection of small, focused projects I built to demonstrate my understanding of ASP.NET Core Web API concepts. Instead of building one large application where everything gets mixed together, I deliberately separated each concept into its own project so that anyone reading this can clearly see what I know and how I think about structuring backend code.

Each project is self-contained, has its own README and can be run independently.

---

## 📁 Repository Structure

```
dotnet-api-showcase/
│
├── 01-product-catalog-api/       # CRUD, EF Core, Repository & Service pattern, DTOs
├── 02-auth-api/                  # JWT, Refresh Tokens, ASP.NET Identity, Role-based auth
├── 03-middleware-api/            # Custom middleware, Caching, Rate limiting, API versioning
├── 04-file-upload-api/           # File handling, IFormFile, streaming, validation
├── 05-realtime-api/              # SignalR, IHostedService, real-time notifications
├── 06-background-jobs-api/       # Hangfire, job queuing, async processing, status tracking
├── 07-minimal-api-comparison/    # Minimal API vs Controller API, endpoint filters, route groups
│
└── README.md                     ← you are here
```

---

## 🧭 Why I Built It This Way

When I started learning .NET Web API, I noticed that most portfolio projects try to do everything in one place - authentication, file uploads, real-time features, background jobs - all inside a single application. The problem I found with that approach is that the concepts become hard to follow. Everything is tangled together and it's difficult to tell what decision was made for what reason.

I wanted to take a different approach. I asked myself: *if someone is reading my code for the first time, what is the clearest way to show them what I understand?*

The answer I landed on was one project per concept. Each project has a single job. It is small enough to read through in one sitting, but complete enough to reflect real architectural decisions. I treated each one as a standalone proof of understanding rather than a feature in a larger app.

I'm also building a larger application separately where all these concepts come together. These smaller projects are how I made sure I understood each concept deeply before combining them.

---

## 🗺️ The Order and Why It Matters

I ordered the projects intentionally. Each one builds on what came before it and I approached learning them in this sequence as well.

```
Foundations          → Project 01 (CRUD, EF Core, clean architecture basics)
       ↓
Security             → Project 02 (Authentication & Authorization)
       ↓
API Design Maturity  → Project 03 (Middleware, Caching, Versioning)
       ↓
Practical Features   → Project 04 (File handling)
       ↓
Real-time            → Project 05 (SignalR, background services)
       ↓
Async Processing     → Project 06 (Background jobs, queuing)
       ↓
Modern .NET          → Project 07 (Minimal APIs, .NET 6+ patterns)
```

Starting with CRUD and EF Core made sense because almost everything else depends on knowing how to structure a project, work with a database and follow REST conventions. From there, adding authentication was the natural next step - before worrying about caching or real-time features, I needed to understand how to secure an API properly.

The later projects cover concepts that I see as differentiators - things that junior developers are not always expected to know but that demonstrate a wider understanding of how production APIs actually behave.

---

## 📦 What Each Project Covers

---

### 01 - Product Catalog API
**Theme:** E-commerce product and category management
**Difficulty:** Beginner → Intermediate

This is the foundation of everything in this repo. I built a product and category management API to cover the concepts that show up in almost every backend role - clean REST design, EF Core Code First and separating the codebase into layers with clear responsibilities.

I chose an e-commerce catalog as the theme because it naturally gives me two related entities (products and categories), which lets me demonstrate foreign keys, navigation properties and filtered queries without over-engineering the domain.

**Concepts covered:**
- RESTful routing and HTTP conventions (`GET`, `POST`, `PUT`, `DELETE`)
- Entity Framework Core - Code First, migrations, one-to-many relationships
- Repository pattern for data access abstraction
- Service layer to keep business logic out of controllers
- DTOs to separate what the database stores from what the API exposes
- Model validation with Data Annotations
- Pagination and search filtering on list endpoints
- Soft delete using an `IsActive` flag instead of hard deletes
- Global exception handling middleware for consistent error responses
- Dependency Injection wired up in `Program.cs`
- Swagger / OpenAPI for interactive documentation
- Database seeding for quick local setup

---

### 02 - Auth API
**Theme:** User registration, login and role-based access control
**Difficulty:** Intermediate

I kept authentication completely separate from the CRUD project on purpose. Mixing auth into the product catalog would have made both harder to understand and harder to explain. This project focuses entirely on the security layer.

The flow I implemented covers everything from registering a user and hashing their password, through to issuing tokens, refreshing them and protecting endpoints based on roles.

**Concepts covered:**
- ASP.NET Core Identity for user management and password hashing
- JWT generation, signing and validation
- Refresh token flow - why short-lived access tokens need a refresh mechanism
- Role-based authorization with `Admin` and `User` roles
- `[Authorize]` attribute and policy-based access control
- Token expiry and a basic revocation strategy
- Protecting individual endpoints vs entire controllers

---

### 03 - Middleware & API Design API
**Theme:** A rate-limited, versioned, cached public API
**Difficulty:** Intermediate

After the first two projects, I felt comfortable writing controllers and handling auth. What I wanted to understand next was everything that happens *around* the business logic - the middleware pipeline, how caching works and how to version an API when it needs to evolve without breaking existing consumers.

This project has no complex domain. The point is the infrastructure layer around it.

**Concepts covered:**
- Writing custom middleware for request logging and response timing
- Built-in .NET 7+ rate limiting - fixed window and sliding window strategies
- In-memory caching with `IMemoryCache`
- API versioning via URL segments and request headers
- Swagger configured per API version
- Health check endpoints
- Response compression

---

### 04 - File Upload & Management API
**Theme:** File storage, retrieval and management
**Difficulty:** Beginner → Intermediate

File handling is something nearly every real application needs but most tutorials skip over. I built this project to fill that gap for myself - understanding how to receive files, validate them, store them safely and serve them back correctly.

I also used this project to think through the architectural difference between storing files locally vs in cloud storage like Azure Blob, even though the implementation here uses local storage.

**Concepts covered:**
- Single and multiple file uploads using `IFormFile`
- Validating file type and file size before accepting uploads
- Streaming large files to avoid excessive memory usage
- Serving files with correct MIME types
- Storing file metadata in a database alongside the physical file
- Sanitizing file names to prevent path traversal vulnerabilities
- Structuring storage in a way that could be swapped to cloud storage

---

### 05 - Real-Time Notifications API
**Theme:** Live notification delivery using SignalR
**Difficulty:** Intermediate → Advanced

Everything up to this point follows the standard HTTP request-response pattern - a client asks for something, the server responds. This project moves into a different model where the server can push updates to connected clients without them asking.

I used a notification system as the domain because it is a realistic use case and simple enough to keep the focus on the real-time mechanics rather than business complexity.

**Concepts covered:**
- SignalR Hub setup and client connection management
- Broadcasting to all clients, targeted users and named groups
- `IHostedService` for a background process that generates and dispatches events
- Handling connect, disconnect and reconnect lifecycle events
- CORS configuration to allow SignalR connections from a browser client
- A minimal JavaScript client included to make the real-time behavior visible

---

### 06 - Order Processing API
**Theme:** Async order placement with background job processing
**Difficulty:** Intermediate → Advanced

This project came from a question I kept asking myself: what happens when a request triggers work that takes too long to complete within a single HTTP response? Holding the connection open is not the answer. The right pattern is to accept the request, queue the work and let the client poll for a status update.

I used an order processing flow as the domain - place an order, the API accepts it immediately, processes it in the background and exposes a status endpoint.

**Concepts covered:**
- Hangfire for background job scheduling, monitoring and retry handling
- Fire-and-forget, delayed and recurring job types
- Job status lifecycle - `Pending → Processing → Completed → Failed`
- `IBackgroundTaskQueue` combined with a hosted worker service
- Retry policies and failure handling for jobs that error out
- The Hangfire dashboard for monitoring running and historical jobs

---

### 07 - Minimal API vs Controller API
**Theme:** A Todo list built twice - side by side
**Difficulty:** Intermediate

.NET 6 introduced Minimal APIs as a new way to write HTTP endpoints without controllers. I wanted to understand the difference between the two approaches properly, so I built the same feature both ways in the same project and compared them directly.

This project is more of a reference and a demonstration of understanding than a standalone application. The value is in the comparison and in being able to talk about which approach fits which scenario.

**Concepts covered:**
- Minimal API routing - `app.MapGet()`, `app.MapPost()`, `app.MapGroup()`
- Endpoint filters as the Minimal API equivalent of action filters
- The full MVC pipeline in controller-based APIs and what it provides
- How middleware behaves differently between the two models
- The tradeoffs - when Minimal APIs make sense and when they don't
- The modern `Program.cs` structure introduced in .NET 6 with no `Startup.cs`

---

## 🛠️ Stack Used Across All Projects

| Technology | Version |
|---|---|
| ASP.NET Core | 10.0.103 |
| Entity Framework Core | 10.0.3 |
| SQL Server | 2025+ / LocalDB |
| Swagger / Swashbuckle | Latest |

### Running any project locally

```bash
# Navigate into the project folder
cd 01-product-catalog-api

# Update the connection string in appsettings.json

# Run migrations
dotnet ef database update

# Start the API
dotnet run

# Open Swagger at https://localhost:{port}/swagger
```

---

## 🗓️ Build Status

| Project | Status |
|---|---|
| 01 - Product Catalog API | 🔄 In Progress |
| 02 - Auth API | 📋 Planned |
| 03 - Middleware API | 📋 Planned |
| 04 - File Upload API | 📋 Planned |
| 05 - Real-Time API | 📋 Planned |
| 06 - Background Jobs API | 📋 Planned |
| 07 - Minimal API Comparison | 📋 Planned |

---

## 👤 About Me

I'm Zayn, a .NET developer building a strong foundation in backend development. This repo reflects how I learn - by isolating concepts, going deep on each one and being deliberate about the decisions I make.

- 🔗 [LinkedIn](https://linkedin.com/in/zaynabid)
- 🐙 [GitHub](https://github.com/zaynabid)