## Notes API

The Notes API is an ASP.NET Core application that provides a simple RESTful API for managing notes. This API allows you to perform basic CRUD (Create, Read, Update, Delete) operations on notes, as well as retrieve notes grouped by their creation date and update date within a specific date range. It uses MySQL as the database for storing notes.

### Prerequisites

Before running the Notes API, you need to ensure that you have the following prerequisites installed:

* **.NET SDK:** Make sure you have the .NET SDK installed on your machine. You can download it from [here](https://dotnet.microsoft.com/en-us/download).
* **MySQL Database:** You must have access to a MySQL database. If you don't have one, please follow the steps below to create it.

**Creating a MySQL Database**

Before you can run the Notes API, you need to create a MySQL database to store the notes. Here are the steps to create a MySQL database:

1. **Access Your MySQL Server:** You should have access to a MySQL server. If you don't have one, you can set up a MySQL server on your local machine or use a cloud-based MySQL service like Azure Database for MySQL.

2. **Connect to MySQL:** Use a MySQL client tool or command-line client to connect to your MySQL server. You will need the username and password for your MySQL server.

```bash
mysql -u your-username -p
```

3. **Create a New Database:** Once you're connected to the MySQL server, you can create a new database using the CREATE DATABASE command. Replace `your-database-name` with the desired name for your database.

```sql
CREATE DATABASE your-database-name;
```


4. **Create a User:** You can create a new user and grant it privileges to access the database. Replace `your-username` and `your-password` with the desired username and password.

```sql
CREATE USER 'your-username'@'%' IDENTIFIED BY 'your-password';
```

5. **Grant Privileges:** Grant the user privileges to the database you created. Replace `your-database-name` and `your-username` with the appropriate values.

```sql
GRANT ALL PRIVILEGES ON your-database-name.* TO 'your-username'@'%';
```

6. **Flush Privileges:** To apply the changes and make sure the user has the necessary privileges, run the following command:

```sql
FLUSH PRIVILEGES;
```

Now, you have successfully created a MySQL database and a user with the required privileges. You can proceed to create the `appsettings.json` file and configure your ASP.NET Core application to use this MySQL database by providing the connection string.

### Installation

Follow these steps to set up and run the Notes API:

1. **Clone the repository to your local machine:**

```bash
git clone git@github.com:MichelWakim/notes-api.git
```
2. **Navigate to the project directory:**
```bash
cd notes-api
```
3. **Create an `appsettings.json` file in the project root and provide your MySQL database connection string.** Here's an example of the content of `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "NotesConnection": "server=your-mysql-server;port=3306;user=your-username;password=your-password;database=your-database-name"
    },
  "Logging": {
   "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

4. **Build the application:**
```bash
dotnet build
```
5. **Apply database migrations to create the required tables:**
```bash
  dotnet ef database update
```
6. **Run the application:**
```bash
dotnet run
```
The API should now be accessible at `https://localhost:7169`.

## Endpoints

The following endpoints are available:

- `GET /api/note`: Get a list of all notes.
- `GET /api/note/{id}`: Get a specific note by its ID.
- `POST /api/note`: Create a new note.
- `PUT /api/note/{id}`: Update an existing note.
- `DELETE /api/note/{id}`: Delete a note by its ID.
- `GET /api/note/grouped-by-created-date`: Get notes grouped by their creation date within a date range.
- `GET /api/note/grouped-by-updated-date`: Get notes grouped by their update date within a date range.

You can use a tool like Swagger to explore and test these endpoints interactively by visiting [https://localhost:5001/swagger](https://localhost:7169/swagger).

## Usage

You can interact with the API using tools like `curl`, [Postman](https://www.postman.com/), or by making HTTP requests from your application.

Here's an example of how to create a new note using `curl`:

```bash
curl -X POST "https://localhost:7169/api/note" -H "Content-Type: application/json" -d '{
  "title": "Sample Note",
  "body": "This is a sample note content."
}'
```
