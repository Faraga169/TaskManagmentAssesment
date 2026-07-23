# Task Management API

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![Repository](https://img.shields.io/badge/repository-Faraga169%2FTaskManagmentAssesment-blue)](https://github.com/Faraga169/TaskManagmentAssesment)

Professional, minimal Task Management API implemented with ASP.NET Core 10 using a Clean Architecture approach. This README is intended for reviewers, evaluators and maintainers and documents setup, API surface, data model, testing and contribution information.

Table of contents
- [Overview](#overview)
- [Badges](#badges)
- [Project structure](#project-structure)
- [Prerequisites](#prerequisites)
- [1 — Setup Instructions](#1---setup-instructions)
  - [Configuration](#configuration)
  - [Database migrations](#database-migrations)
  - [Run locally](#run-locally)
  - [Swagger / OpenAPI](#swagger--openapi)
- [2 — API Documentation](#2---api-documentation)
  - [Authentication notes](#authentication-notes)
  - [Implemented endpoints (summary)](#implemented-endpoints-summary)
  - [Request / response examples](#request--response-examples)
  - [Filtering, pagination and sorting](#filtering-pagination-and-sorting)
- [3 — Database Schema Explanation](#3---database-schema-explanation)
- [4 — Test Instructions](#4---test-instructions)
  - [Run tests](#run-tests)
  - [What tests verify](#what-tests-verify)
- [Postman collection](#postman-collection)
- [Author](#author)

Overview
--------
This API provides a task and project management backend exposing endpoints for user registration/authentication, project CRUD and task CRUD operations. The codebase follows Clean Architecture conventions with separate projects for Presentation (API), Business Logic, Data Access and Tests.

Badges
------
- .NET 10.0 — runtime/target framework
- Repository / license badge — quick reference

Project structure
-----------------
Paths and project names used in this repository (as present in the source):

- Presentation Layer (API) — ASP.NET Core Web API (Presentation)
- Data Layer (DAL) — EF Core persistence, entities, configurations, migrations
- Business Layer (BLL) — business logic, DTOs, services, validators
- Unit Tests — unit tests
- Integration Tests— integration tests

Prerequisites
-------------
- .NET 10 SDK
- SQL Server (local instance or container)

1 — Setup Instructions
----------------------

1. Clone repository

```powershell
git clone https://github.com/Faraga169/TaskManagmentAssesment.git "D:\ITI\Task Managment"
cd "D:\ITI\Task Managment"
```

2. Restore packages

```powershell
dotnet restore
```

Configuration
-------------
Edit the Presentation project's configuration (Task Managment.PL/appsettings.Development.json or appsettings.json). At minimum configure a SQL Server connection string and JWT settings.

Example appsettings.Development.json (replace values):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskManagmentDb;User Id=sa;Password=YOUR_STRONG_PASSWORD;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "Key": "Your_Very_Long_Symmetric_Key_At_Least_32_Chars",
    "Issuer": "TaskManagementAPI",
    "Audience": "TaskManagementClient",
    "DurationInMinutes": 60
  }
}
```

Notes
- JwtSettings maps to the project's JwtSettings helper (Task_Managment.BLL.Helper.JwtSettings).
- The application registers JsonStringEnumConverter so enums are serialized/deserialized as strings.

Database migrations
-------------------
Apply EF Core migrations from the repository root. Example command:

```powershell
dotnet ef database update --project "Task Managment.DAL/Task Managment.DAL.csproj" --startup-project "Task Managment.PL/Task Managment.API.csproj"
```

Run locally
-----------
Start the API:

```powershell
dotnet run --project "Task Managment.PL/Task Managment.API.csproj"
```

Swagger / OpenAPI
-----------------
In Development the project exposes Swagger UI. Open:

http://localhost:{port}/swagger

(Port is shown by the `dotnet run` output.)

2 — API Documentation
---------------------

Authentication notes
- The API uses JWT authentication. Login and register endpoints are unauthenticated.
- Endpoints that require authentication are decorated with [Authorize].
- To call protected endpoints include header: Authorization: Bearer <jwt-token>

Implemented endpoints (summary)
- POST /api/auth/register — Register new user (no auth)
- POST /api/auth/login — Login and receive JWT token (no auth)
- GET /api/project — List paginated projects (authenticated)
- GET /api/project/{id} — Get project details (authenticated)
- POST /api/project — Create a project (authenticated)
- PUT /api/project/{id} — Update a project (authenticated)
- DELETE /api/project/{id} — Soft-delete a project (authenticated)
- GET /api/project/{id}/tasks — Get tasks for a project (authenticated)
- POST /api/project/{id}/tasks — Create task under a project (authenticated)
- GET /api/task — List paginated tasks (authenticated)
- GET /api/task/{id} — Get task details (authenticated)
- PUT /api/task/{id} — Update a task (authenticated)
- DELETE /api/task/{id} — Soft-delete a task (authenticated)

Request / response examples
- Registration (RegisterDTO)

Request (POST /api/auth/register):

```json
{
  "UserName": "alice01",
  "Email": "alice@example.com",
  "Password": "P@ssw0rd1!"
}
```

Response (AuthResponseDto):

```json
{
  "Token": "<jwt-token>",
  "Expiration": "2026-07-25T15:00:00Z"
}
```

- Login (LoginDTO)

Request (POST /api/auth/login):

```json
{
  "Email": "alice@example.com",
  "Password": "P@ssw0rd1!"
}
```

Response (AuthResponseDto):

```json
{
  "Token": "<jwt-token>",
  "Expiration": "2026-07-25T15:00:00Z"
}
```

- Create Project (CreateAndUpdateProjectDto)

Request (POST /api/project):

```json
{
  "Name": "My Project",
  "Description": "Project description"
}
```

Response (ProjectDto):

```json
{
  "Id": 1,
  "Name": "My Project",
  "Description": "Project description"
}
```

- Create Task (CreateTaskDto) — POST /api/project/{id}/tasks

Request:

```json
{
  "Title": "Implement API",
  "Description": "Implement endpoints",
  "Priority": "High",
  "DueDate": "2026-08-01"
}
```

Response (TaskDTO):

```json
{
  "Id": 42,
  "Title": "Implement API",
  "Status": "Todo",
  "Priority": "High",
  "DueDate": "2026-08-01",
  "ProjectName": "My Project"
}
```

- Get Task details (GET /api/task/{id}) returns TaskDetailsDTO:

```json
{
  "Id": 42,
  "Title": "Implement API",
  "Description": "Implement endpoints",
  "Status": "Todo",
  "Priority": "High",
  "DueDate": "2026-08-01",
  "ProjectId": 1,
  "ProjectName": "My Project",
  "CreatedAt": "2026-07-25T14:20:00Z"
}
```

Filtering, pagination and sorting
- Task and Project listing endpoints use specification parameter objects.
- TaskSpecParams supports:
  - pageIndex (int)
  - pageSize (int — capped at 20)
  - search (string)
  - status (TaskStatus enum, passed as string)
  - priority (TaskPriority enum, passed as string)
  - dueDateFrom / dueDateTo (DateOnly)
  - sort (example: dueDate, priority, title)

Example call:

```
GET /api/task?pageIndex=1&pageSize=5&search=API&status=Todo&priority=High&sort=dueDate
Authorization: Bearer <token>
```

Response is PaginationDTO<TaskDTO> with PageIndex, PageSize, Count, Data[].

3 — Database Schema Explanation
-------------------------------

Core entities (implemented under Task Managment.DAL/Presisitence/Models):

- AspNetUsers (Identity) — PK: Id (string). Navigation: ApplicationUser.Projects.
- Projects (Project entity)
  - Id (int, PK)
  - Name (nvarchar(100))
  - Description (nvarchar(1000))
  - OwnerId (nvarchar(450), FK -> AspNetUsers.Id)
  - CreatedAt, UpdatedAt, IsDeleted, DeletedAt
- Tasks (Task entity)
  - Id (int, PK)
  - ProjectId (int, FK -> Projects.Id)
  - Title (nvarchar(150))
  - Description (nvarchar(2000))
  - Status (stored as string)
  - Priority (stored as string)
  - DueDate (date)
  - CreatedAt, UpdatedAt, IsDeleted, DeletedAt

Relationships and behaviors
- One ApplicationUser -> many Projects (OwnerId FK).
- One Project -> many Tasks (ProjectId FK).
- Status and Priority enums are persisted as strings (configured in TaskConfiguration).
- Soft-deletes are implemented using IsDeleted and DeletedAt on a BaseEntity; the DbContext applies a global query filter to hide soft-deleted records.

Rationale
- Ownership model: user -> projects -> tasks, with soft-delete and common query filters to support spec-based queries for status, priority and due date.

4 — Test Instructions
---------------------

Test projects present in the repository:

- Unit tests: TaskManagement.Tests (TaskServiceTests.cs, ProjectServiceTests.cs)
- Integration tests: Task_Management.Tests.Integration (ProjectFlowTests.cs, TaskFilterTests.cs, TaskSearchTests.cs)

Run all tests

```powershell
dotnet test
```

Run only unit tests

```powershell
dotnet test "TaskManagement.Tests/TaskManagement.Tests.Unit.csproj"
```

Run only integration tests

```powershell
dotnet test "Task_Management.Tests.Integration/Task_Management.Tests.Integration.csproj"
```

What tests verify (brief)
- Unit tests: service-level logic and validation (project creation, duplicate-name validation, task service behavior).
- Integration tests: end-to-end API scenarios (register/login, create project, add tasks, update task status, delete project; filtering/search/pagination flows).
## Postman Collection

A Postman collection is included in the repository to simplify API testing.

### Location

```text
postman/TaskManagement.postman_collection.json
```

### Usage

1. Import the collection into Postman.
2. Execute the **Register** request to create a new user.
3. Execute the **Login** request to obtain a JWT access token.
4. Set the returned JWT as a **Bearer Token** in the collection Authorization settings (or add it manually to the `Authorization` header).
5. Test the remaining protected endpoints, including Project and Task operations.

### Authorization Header

```http
Authorization: Bearer <your-jwt-token>
```

The collection contains requests for:

- Authentication (Register & Login)
- Project CRUD Operations
- Task CRUD Operations
- Task Filtering
- Task Searching
- Pagination

Author
------
Repository owner / author: Faraga169 — https://github.com/Faraga169




