# BookStore API

A RESTful Web API for managing books and orders.

## Technologies Used
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Repository Pattern
- Unit Of Work Pattern

## Features
- CRUD Operations for Books
- DTO Pattern for Data Transfer
- Soft Delete Support
- Pagination
- Database Migrations
- Clean Architecture Structure

## Project Structure

- BookStore.API → API Endpoints & Controllers
- BookStore.Data → DbContext & Database Configuration
- BookStore.Model → Domain Entities
- BookStore.DTO → Data Transfer Objects (CreateBookDTO, BookDTO)
- BookStore.Repository → Repository Implementations
- BookStore.UnitOfWork → Unit Of Work Implementation

## Getting Started

1. Clone the repository
2. Update the connection string in appsettings.json
3. Run migrations
4. Start the application

## Author

Mohamed Elsayed
