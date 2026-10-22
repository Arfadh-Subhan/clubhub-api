# ClubHub - Backend API

A robust RESTful API built with **ASP.NET Core** and **Entity Framework Core** that powers the ClubHub campus management platform. It handles user authentication, club management, event approvals, merchandise orders, and real-time chat notifications.

##  Tech Stack
*   **Framework:** ASP.NET Core Web API (.NET 8/9)
*   **Database:** SQL Server (via Entity Framework Core)
*   **Authentication:** BCrypt (Password Hashing) + Session-based tracking
*   **Documentation:** Scalar / OpenAPI

##  Key Features
*   **Role-Based Access Control (RBAC):** 4 distinct roles (Student, Executive, ICBT Admin, Site Admin).
*   **Club Management:** Create clubs, manage memberships, and handle join requests.
*   **Approval Workflows:** Multi-step approvals for Events, Posts, Products, and Club Requests.
*   **E-Commerce:** Product listings and order tracking with stock management.
*   **Campus Chat:** A built-in chat system restricted to approved club members.
*   **Analytics:** Real-time statistics for ICBT Admins (active clubs, pending reviews, etc.).

##  Getting Started
1. Clone the repository.
2. Update the `DefaultConnection` string in `appsettings.json` to point to your SQL Server.
3. Run `dotnet ef database update` to apply migrations.
4. Run `dotnet run` to start the API.
5. Access the API documentation at `https://localhost:<port>/scalar/v1`.

##  Project Structure
*   `Controllers/` - API endpoints
*   `Services/` - Business logic and database interactions
*   `Models/` - Entity Framework database models
*   `DTOs/` - Data Transfer Objects and custom validation attributes
*   `Data/` - Database context and configurations