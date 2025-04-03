# ECommerceAPI

## Overview
This project is a simple **EcommerceAPI** built using **ASP.NET Core 8** and **Entity Framework Core** with **SQLite**. It follows a layered architecture and implements key features such as:
- **JWT Authentication**
- **Shopping Cart & Order Processing**
- **Product CRUD**
- **Mock Payment Gateway** -- Not yet done, :D

## Features
- **User Authentication:** Secure login with JWT token.
- **Admin Roles:** CRUD management of products/items
- **Shopping Cart:** Add items to a cart and store them in the database.
- **Order Creation:** Convert a cart into an order with order details.
- **Order Retrieval:** Fetch order details by Order ID.
- **Mock Payment Processing:** Simulates payment and updates order status.

## Project Structure
```
ECommerceAPI/
│-- Api/                # API Controllers
│-- Application/        # Business Logic & Services
│-- Domain/            # Entities & Core Models
│-- Infrastructure/    # Data Access & Repositories
│-- README.md          # Project Documentation
```

## Setup Instructions
### 1. Clone the Repository
```sh
git clone https://github.com/your-repo/order-management-api.git
cd order-management-api
```

### 2. Install Dependencies
```sh
dotnet restore
```

### 3. Configure Database
- The API uses **SQLite**. Ensure the connection string is set in `appsettings.json`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=order.db"
  }
  ```
  -DB has pre store values for users. admin and customer

### 4. Run the Application
```sh
dotnet run
```

## API Endpoints
### **Authentication**
| Method | Endpoint         | Description          |
|--------|-----------------|----------------------|
| POST   | `/api/auth/login` | Authenticate & get JWT token |   

### **Shopping Cart**
| Method | Endpoint                   | Description          |
|--------|-----------------------------|----------------------|
| POST   | `/api/cart/add`              | Add an item to the cart |
| GET    | `/api/cart/{cartId}`         | Get cart details |

### **Orders**
| Method | Endpoint                   | Description          |
|--------|-----------------------------|----------------------|
| POST   | `/api/orders/create`        | Create an order from cart |
| GET    | `/api/orders/{orderId}`     | Get order by ID |
| PUT    | `/api/orders/pay/{orderId}` | Process payment & update order |

## Mock Payment Processing
A simple mock payment service updates the order status from **PENDING** to **PAID** upon successful transaction.

## Next Steps
- Improve error handling and validation.
- Add logging mechanism
- Add unit tests for services and controllers.
- Proper API Documentation
- Refactoring and Apply DRY

---
**Author:** Allan Robert Jaranilla

