# Blog Application

This is a full-stack Blog Application built using the following technologies:

- **Backend**: .NET 10 Web API with Entity Framework Core, Dapper, and ADO.NET
- **Frontend**: React with Vite, Tailwind CSS, and Axios

## Features

- User authentication and authorization
- CRUD operations for blogs
- Responsive design using Tailwind CSS
- API integration with Axios
- Multiple database access strategies: EF Core, Dapper, and ADO.NET

---

## Prerequisites

Before running the application, ensure you have the following installed:

- [Node.js](https://nodejs.org/) (LTS recommended)
- [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)
- [.NET SDK 10](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

## Installation

### Backend Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/SSujjall/BlogApp.git
   cd blog-app/backend
   ```

2. Restore .NET dependencies:
   ```bash
   dotnet restore
   ```

3. Update `appsettings.json` with your database connection string:
   ```json
   "ConnectionStrings": {
       "BlogDB": "Your-SQL-Server-Connection-String"
   }
   ```

4. Apply EF Core migrations:
   ```bash
   dotnet ef database update
   ```

5. Run the backend:
   ```bash
   dotnet run
   ```

### Frontend Setup
1. Navigate to the frontend directory:
   ```bash
   cd ../BlogApp.ReactFrontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm run dev
   ```

---

### YOU NEED .ENV FILE IN ROOT DIRECTORY FOR RUNNING IN LOCAL ENVIRONMENT.
.env FILE TEMPLATE:
```bash
   VITE_GOOGLE_CLIENT_ID=your-google-client-id
   VITE_API_BASE_URL=https://localhost:7108/api
   VITE_PROD_API_BASE_URL=your-production-api-base-url
```
