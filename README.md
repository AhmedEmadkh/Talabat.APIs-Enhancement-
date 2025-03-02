# Talabat API

## Overview
Talabat API is a **scalable and modular ASP.NET Core Web API** designed using **Onion Architecture** for better maintainability and flexibility. It provides **secure and efficient** management of orders, products, and payments with role-based authentication and integrated transaction handling.

## Features
- 🛒 **Order Management** – Create, update, and track orders seamlessly.
- 📦 **Product Management** – Manage products with CRUD operations.
- 💳 **Payment Integration** – Secure transaction processing.
- 🔐 **Role-Based Authentication** – User and admin role management.
- 🏗 **Onion Architecture** – Ensures separation of concerns and maintainability.
- 📡 **RESTful API** – Follows REST principles for scalability.

## Technologies Used
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Repository & Unit of Work Pattern**
- **Onion Architecture**
- **Swagger for API Documentation**

## Installation
### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Visual Studio / VS Code

### Steps
1. Clone the repository:
   ```sh
   git clone https://github.com/AhmedEmadkh/Talabat-API.git
   cd Talabat-API
   ```
2. Configure the database connection in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TalabatDB;Trusted_Connection=True;"
   }
   ```
3. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
4. Run the application:
   ```sh
   dotnet run
   ```

## API Endpoints
### Authentication
- `POST /api/auth/register` – Register a new user
- `POST /api/auth/login` – Authenticate and get a JWT token

### Products
- `GET /api/products` – Retrieve all products
- `POST /api/products` – Add a new product
- `PUT /api/products/{id}` – Update product details
- `DELETE /api/products/{id}` – Remove a product

### Orders
- `GET /api/orders` – Retrieve all orders
- `POST /api/orders` – Create a new order
- `PUT /api/orders/{id}` – Update an order
- `DELETE /api/orders/{id}` – Cancel an order

## Contributing
1. Fork the repository.
2. Create a feature branch: `git checkout -b feature-name`
3. Commit changes: `git commit -m "Add new feature"`
4. Push to the branch: `git push origin feature-name`
5. Open a Pull Request.

## License
This project is licensed under the **MIT License**.

---

**🚀 Developed by Ahmed Emad**
