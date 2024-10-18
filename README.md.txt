# PersonAPI .NET Application

This is a .NET application for managing a database of `personas`, `profesiones`, `estudios`, and `telefonos` using a SQL Server database. The application supports basic CRUD operations and is containerized using Docker.

## Prerequisites

Make sure you have the following installed on your machine:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8 or above)
- [Docker](https://www.docker.com/get-started) (latest version)

## Running the Application

You can run the application using Docker Compose, which will build the .NET application and set up a SQL Server database. The database schema is automatically applied upon container start.

### Steps to Run:

1. **Clone this repository:**

git clone https://github.com/KJAIMESC/personapi-dotnet.git
cd <your-repo-directory>


2. **Start the application using Docker Compose:**

docker-compose up --build


This command will:

- Build the .NET application.
- Set up a SQL Server database.
- Automatically apply the schema and seed some initial data.

3. **Access the Front:**

- The API will be available at: `http://localhost:5000`
- Swagger UI will be available at: `http://localhost:5000/swagger`
