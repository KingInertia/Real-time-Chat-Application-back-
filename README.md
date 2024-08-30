# Real-time Chat Application ## Running the Backend

To run the backend of your application, follow these steps:

1. **Rename the `exampleAppsettings.json` file to `Appsettings.json`.**

   This file contains the application's configuration, including connection strings and other settings.

2. **Update the connection strings in `Appsettings.json`.**

   Open the `Appsettings.json` file and replace the connection strings with your own values. Make sure to provide the correct parameters for connecting to your database and other services.

3. **Apply migrations to the database.**

   Run the following command to apply migrations and update the database:

   ```bash
   dotnet ef database updat

4. **Start the project using the command**
   
   dotnet run
