using CommissionManagerAPP.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using CommissionManagerAPP.Exceptions;
using Microsoft.Data.Sqlite;

namespace CommissionManagerAPP.Repositories
{
    public class UserRepository : IUserRepository
    {
        //Single Responsibility principle - handles operations related to userProfiles
        private readonly IConfiguration _configuration;

        private const string connectionStringName = "DefaultConnection";

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Called to save profile, also called by CreateProfileAsync
        public async Task<bool> SaveUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                Guid userId = Guid.NewGuid();

                userProfile.Id = userId;

                string connectionString = _configuration.GetConnectionString(connectionStringName);

                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"INSERT INTO UserProfiles (Id, Username, Email, Password, CreatedDate) VALUES (@Id, @Username, @Email, @Password, @CreatedDate)";

                    await connection.ExecuteAsync(sql, userProfile);
                    await connection.CloseAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred in SaveUserProfileAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<UserProfile> GetUserProfileByEmailAsync(string email)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString(connectionStringName);

                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    UserProfile user = await connection.QuerySingleOrDefaultAsync<UserProfile>("SELECT * FROM UserProfiles WHERE Email = @Email", new { Email = email });

                    if (user == null)
                    {
                        throw new UserNotFoundException($"User with email {email} not found");
                    }

                    return user;
                }
            }
            catch (UserNotFoundException)
            {
                // Log the specific exception and handle accordingly
                Console.WriteLine($"UserNotFoundException in GetUserProfileByEmailAsync");
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred in GetUserProfileByEmailAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                bool success = await SaveUserProfileAsync(userProfile);

                if (success)
                {
                    return userProfile;
                }
                else
                {
                    throw new Exception("SaveUserProfileAsync failed");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred in CreateUserProfileAsync: {ex.Message}");
                throw;
            }
        }
    }

}
