using CommissionManagerAPP;
using Microsoft.Data.SqlClient;
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

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
    }
}
