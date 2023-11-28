using CommissionManagerAPP;
using Dapper;
using Microsoft.Data.SqlClient;
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

            if (!File.Exists("Data Source=commissionManager.db"))
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
            using (SqlConnection connection = new SqlConnection(connectionString))
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
                    Console.WriteLine("Database connection failed: " + ex.Message);

                    return false;
                }
            }
        }

        public static void SetUpDatabase()
        {
            using var connection = new SqliteConnection("Data Source=commissionManager.db");
            connection.Open();

            connection.Execute(@"
        CREATE TABLE Commissions (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            CommissionedDate DATETIME NOT NULL,
            Deadline DATETIME NOT NULL,
            Description TEXT,
            ClientId GUID NOT NULL,
            Status TEXT NOT NULL,
            Email TEXT
        )
    ");

            connection.Execute(@"
        INSERT INTO Commissions (CommissionedDate, Deadline, Description, ClientId, Status, Email)
        VALUES (@CommissionedDate, @Deadline, @Description, @ClientId, @Status, @Email)",
                new
                {
                    CommissionedDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(14),
                    Description = "Example commission description",
                    ClientId = Guid.NewGuid(),
                    Status = "Pending",
                    Email = "example@email.com"
                });

            connection.Execute(@"
        CREATE TABLE UserProfiles (
            Id GUID PRIMARY KEY,
            Username TEXT NOT NULL,
            Email TEXT NOT NULL UNIQUE,
            CreatedDate DATETIME NOT NULL,
            Password TEXT NOT NULL
        )
    ");

            connection.Execute(@"
        INSERT INTO UserProfiles (Id, Username, Email, CreatedDate, Password)
        VALUES (@Id, @Username, @Email, @CreatedDate, @Password)",
                new
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Email = "admin@example.com",
                    CreatedDate = DateTime.Now,
                    Password = "password"
                });
        }
    }
}
