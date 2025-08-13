# LedgerlyAPI

LedgerlyAPI is an ASP.NET Core Web Api that provides the backend services for the Ledgerly React application.
It has multiple API end points that persist, or grab data from an MSSQL database, and uses JWT (JSON Web Tokens) for authorization and authentication.

## Requirements
- .NET 9 SDK
- SQL Server
- Visual Studio / VS Code / Rider
- Git

## Getting Started
1. Clone the repository:
```
git clone https://github.com/sp0389/LedgerlyApi.git
```
```
cd LedgerlyApi
```
3. Run database migrations:
```
dotnet ef database update
```
4. Start the application:
```
dotnet run or run the application from the IDE.
```
## Usage
- Access via (Swagger):
```
https://localhost:7156/swagger/index.html (or configured port).
```
Postman:
```
https://localhost:7156/api/{controller}/{action}
```
## Deployment
```
dotnet publish -c Release
```
Deploy to Azure, IIS, or containerize with Docker.

## Contributing
1. Fork the repository.
2. Create a feature branch.
3. Commit changes.
4. Open a Pull Request.
