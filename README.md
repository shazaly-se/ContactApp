📇 ContactApp
A full-stack ASP.NET Core MVC CRUD application for managing contacts, deployed on Microsoft Azure with a complete CI/CD pipeline using GitHub Actions.

🚀 Features
Create, Read, Update, Delete (CRUD) contacts
Upload and manage profile images using Azure Blob Storage
Store data in Azure SQL Database
Automatic database migrations
CI/CD pipeline with GitHub Actions
Deployed on Azure App Service
🧰 Tech Stack
ASP.NET Core MVC (.NET 10)
Entity Framework Core
Azure App Service
Azure SQL Database
Azure Blob Storage
GitHub Actions (CI/CD)
🏗️ Architecture Overview
Frontend: Razor Views (MVC)
Backend: ASP.NET Core MVC
Database: Azure SQL (EF Core Code First)
Storage: Azure Blob Storage (for images)
Deployment: GitHub Actions → Azure App Service
⚙️ Configuration
1. Clone the repository
git clone https://github.com/shazaly-se/ContactApp.git
cd ContactApp
2. Update appsettings.json
"ConnectionStrings": {
  "Default": "YOUR_SQL_CONNECTION_STRING"
},
"AzureBlob": {
  "ConnectionString": "YOUR_BLOB_CONNECTION_STRING",
  "ContainerName": "contactimages"
}
3. Run migrations
dotnet ef database update
4. Run the application
dotnet run
☁️ Azure Deployment
This project is deployed using:

Azure App Service → hosts the web app
Azure SQL Database → stores contact data
Azure Blob Storage → stores profile images
🔄 CI/CD Pipeline (GitHub Actions)
Triggered on push to main / master

Steps:

Build application
Publish artifacts
Deploy to Azure App Service
Workflow file:

.github/workflows/main.yml
🔐 Security Notes
Secrets (connection strings, keys) are stored in:

Azure App Service (Environment Variables)
GitHub Secrets
Sensitive data is NOT stored in source code

📌 Future Improvements
Add authentication (ASP.NET Identity / JWT)
Add API version (REST API)
Add caching (Redis)
Add logging & monitoring
👨‍💻 Author
Andandy, Shazaly
