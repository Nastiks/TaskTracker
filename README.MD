# Project Task Tracker

This project was created for the purpose of tracking projects and the tasks they contain. Each user can use the project to track the status of assigned tasks.


# Technologies

Entity Framework (EF) Core, ASP.NET Core 7, C#, SQLite, PostgreSQL

## Libraries used

Gridify, OpenAPI, EFCore.Proxies, EFCore.SQLite, Npgsql

## Using

Use git clone to clone the repository:
**git clone https://github.com/Nastiks/TaskTracker.git**
To run the executable files in the **Build** folder, select the appropriate operating system
Run the file **WebTasks.Server.exe**
If you want to use Postgresql, then in the configuration file set false on the parameter **"UseLocalSqliteDb"** and fill in the data about the SQL server in the same file
If desired, you can use the docker file available in the repository
All available API documentation can be found at /doc
