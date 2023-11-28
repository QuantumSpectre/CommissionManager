using CommissionManagerAPP;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CommissionManagerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            if (File.Exists("Data Source=commissionManager.db") == false)
            {
                SetUpDatabase();
            }

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }

            if (TestConnection(connectionString))
            {
                Console.WriteLine("Succesfully Connecting");

                CreateHostBuilder(args).Build().Run();
            }
            else
            {
                Console.WriteLine("Exiting due to a database connection failure.");
            }
        }
        //the actual method that starts the web api hence the webbuilder method being called
        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });


        //A simple test connection for my own satisfaction
        static bool TestConnection(string connectionString)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    Console.WriteLine("Database connection successful!");

                    connection.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database connection failed: " + ex.ToString());

                    return false;
                }
            }
        }

        public static void SetUpDatabase()
        {
            using var connection = new SqliteConnection("Data Source=commissionManager.db");
            connection.Open();

            if (!TableExists(connection, "Commissions"))
            {
                connection.Execute(@"
        CREATE TABLE Commissions (
            Id GUID PRIMARY KEY NOT NULL,
            CommissionedDate DATETIME NOT NULL,
            Deadline DATETIME NOT NULL,
            Description TEXT,
            ClientId GUID NOT NULL,
            Status TEXT NOT NULL,
            Email TEXT
        )
    ");
            }

            if (!TableExists(connection, "UserProfiles"))
            {
                connection.Execute(@"
        CREATE TABLE UserProfiles (
            Id GUID PRIMARY KEY,
            Username TEXT NOT NULL,
            Email TEXT NOT NULL UNIQUE,
            CreatedDate DATETIME NOT NULL,
            Password TEXT NOT NULL
        )
    ");
            }
        }

        private static bool TableExists(SqliteConnection connection, string tableName)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @tableName";
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = command.ExecuteReader();
            if (reader.Read() && reader.GetInt32(0) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
