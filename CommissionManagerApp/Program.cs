using CommissionManagerAPP;
using CommissionManagerAPP.Models;
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

            SqlMapper.AddTypeHandler(new GuidHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "commissionManager.db")) == false)
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

            // Generate new guids
            Guid commissionId1 = Guid.NewGuid();
            Guid commissionId2 = Guid.NewGuid();
            Guid commissionId3 = Guid.NewGuid();
            Guid commissionId4 = Guid.NewGuid();
            Guid clientid1 = Guid.NewGuid();
            Guid clientid2 = Guid.NewGuid();
            Guid clientid3 = Guid.NewGuid();
            Guid clientid4 = Guid.NewGuid();

            connection.Execute(@"
        INSERT INTO Commissions (Id, CommissionedDate, Deadline, Description, ClientId, Status, Email)
        VALUES
            (@Id1, '2023-11-27', '2023-12-15', 'Star Wars-themed commission', @ClientId1, 'In Progress', 'example@email.com'),
            (@Id2, '2023-11-28', '2023-12-20', 'The Matrix-inspired artwork', @ClientId2, 'Queued', 'example@email.com'),
            (@Id3, '2023-11-29', '2023-12-25', 'Super Mario Bros. caricature', @ClientId3, 'In Progress', 'example@email.com'),
            (@Id4, '2023-12-01', '2023-12-30', 'Avengers Endgame fan art', @ClientId4, 'Queued', 'example@email.com')
    ", new { Id1 = commissionId1, Id2 = commissionId2, Id3 = commissionId3, Id4 = commissionId4, ClientId1 = clientid1 , ClientId2 = clientid2 , ClientId3 = clientid3, ClientId4 = clientid4 , });

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

            // Generate a new GUID for the user profile
            Guid userProfileId = Guid.NewGuid();
            string Securepassword = BCrypt.Net.BCrypt.HashPassword("SecurePassword");

            connection.Execute(@"
        INSERT INTO UserProfiles (Id, Username, Email, CreatedDate, Password)
        VALUES
            (@Id, 'ExampleUser', 'example@email.com', '2023-11-27', @securepassword)
    ", new { Id = userProfileId , securepassword = Securepassword });
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
