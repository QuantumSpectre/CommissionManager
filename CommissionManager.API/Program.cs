using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CommissionManager.API;

internal class Program
{
    static void Main(string[] args)
    {
        //sets the config file allowing connection string to be used
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        if (TestConnection(connectionString))
        {
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