## Overview
This project is a full-stack application that consists of a minimal API built with ASP.NET and a frontend developed using Blazor. The API provides endpoints for fetching and filtering product data, while the Blazor frontend displays the data in a table with sorting and filtering functionalities.

## Technologies Used
### Backend:
- ASP.NET Core Web API
- Entity Framework Core
- SQL Database
- Dependency Injection (DI) for service management

### Frontend:
- Blazor Webserver
- Bootstrap for styling

## API Endpoints
The API provides the following endpoints:

| Method | Endpoint | Description |
|--------|---------|-------------|
| GET | `/products` | Retrieves all products |
| GET | `/products/{ean}` | Retrieves a product by its EAN |
| GET | `/products/size/{size}` | Retrieves products of a specific size |
| GET | `/products/sort/price?ascending=false` | Retrieves products sorted by price (descending) |
| GET | `/products/sort/price?ascending=true` | Retrieves products sorted by price (ascending) |
| GET | `/products/instock` | Retrieves only in-stock products |

## Database & Models
- The backend models were created based on the database schema.
- `DbContext` was set up accordingly to manage database interactions.
- A `ProductDTO` was implemented to shape the data in the format expected by the frontend.
- Dependency Injection (DI) is used to manage services and database access.

## Frontend Functionality
- Fetches data from the API and displays it in a table using Blazor components.
- Includes buttons for sorting by price (ascending/descending).
- Provides a button to filter and display only in-stock products.
- Styled using Bootstrap for a responsive and clean UI.

## Setup & Installation
### Backend:
1. Clone the repository.
2. Navigate to the backend folder and run:
   ```sh
   dotnet run
   ```
3. Ensure the database connection is configured properly in `appsettings.json`.

### Frontend:
1. Navigate to the Blazor frontend folder.
2. Run the frontend application with:
   ```sh
   dotnet run
   ```
3. Open a browser and navigate to the appropriate localhost URL.

## Future Improvements
- Add authentication and user roles.
- Implement a search feature.
- Enhance UI/UX for a better user experience.

