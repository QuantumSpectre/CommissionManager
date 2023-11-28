using CommissionManagerAPP.Exceptions;
using CommissionManagerAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CommissionManagerAPP.Repositories
{
    //single responsibility principle - Handles operations related to commissions
    public class CommissionRepository : ICommissionRepository
    {
        private readonly IConfiguration _configuration;

        public CommissionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Commission>> GetCommissionsAsync()
        {
            try
            {
                string sql = "SELECT * FROM Commissions";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                List<Commission> commissions = new List<Commission>();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    commissions = (await connection.QueryAsync<Commission>(sql)).ToList();

                    await connection.CloseAsync();
                }

                return commissions;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in GetCommissionsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Commission>> GetCommissionByEmailAsync(string email)
        {
            try
            {
                List<Commission> commissions;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    commissions = (await connection.QueryAsync<Commission>("SELECT * FROM Commissions WHERE Email = @Email", new { Email = email })).ToList();

                    if (commissions.Count == 0)
                    {
                        throw new CommissionNotFoundException($"No commissions found with email {email}");
                    }
                }

                return commissions;
            }
            catch (CommissionNotFoundException)
            {
                // Logging
                Console.WriteLine($"CommissionNotFoundException in GetCommissionByEmailAsync");
                throw;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in GetCommissionByEmailAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Commission> GetCommissionByIdAsync(Guid id)
        {
            try
            {
                Commission commission;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    commission = await connection.QuerySingleOrDefaultAsync<Commission>("SELECT * FROM Commissions WHERE Id = @Id", new { Id = id });

                    if (commission == null)
                    {
                        throw new CommissionNotFoundException($"Commission with Id {id} not found");
                    }
                }

                return commission;
            }
            catch (CommissionNotFoundException)
            {
                // Logging
                Console.WriteLine($"CommissionNotFoundException in GetCommissionByIdAsync");
                throw;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in GetCommissionByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Guid> CreateCommissionAsync(Commission commission)
        {
            try
            {
                Guid commissionId = Guid.NewGuid();

                string sql = @"
            INSERT INTO Commissions (Id, Description, ClientID, CommissionedDate, Deadline, Status, Email)
            VALUES (@Id, @Description, @ClientId, @CommissionedDate, @Deadline, @Status, @Email);";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(sql, new
                    {
                        Id = commissionId,
                        commission.Description,
                        commission.ClientId,
                        commission.CommissionedDate,
                        commission.Deadline,
                        commission.status,
                        commission.email
                    });

                    await connection.CloseAsync();

                    return commissionId;
                }
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in CreateCommissionAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateCommissionAsync(Guid id, Commission updatedCommission)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var existingCommission = await connection.QuerySingleOrDefaultAsync<Commission>("SELECT * FROM Commissions WHERE Id = @Id", new { Id = id });

                    if (existingCommission == null)
                    {
                        throw new CommissionNotFoundException($"Commission with ID {id} not found");
                    }

                    string updateSql = @"
                    UPDATE Commissions
                    SET Description = @Description,
                        ClientId = @ClientId,
                        CommissionedDate = @CommissionedDate,
                        Deadline = @Deadline,
                        Status = @Status
                    WHERE Id = @Id;
                ";

                    await connection.ExecuteAsync(updateSql, updatedCommission);

                    await connection.CloseAsync();
                }
            }
            catch (CommissionNotFoundException)
            {
                // Logging
                Console.WriteLine($"CommissionNotFoundException in UpdateCommissionAsync");
                throw;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in UpdateCommissionAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteCommissionAsync(Guid id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var existingCommission = await connection.QuerySingleOrDefaultAsync<Commission>("SELECT * FROM Commissions WHERE Id = @Id", new { Id = id });

                    if (existingCommission == null)
                    {
                        throw new CommissionNotFoundException($"Commission with ID {id} not found");
                    }

                    string deleteSql = "DELETE FROM Commissions WHERE Id = @Id";
                    await connection.ExecuteAsync(deleteSql, new { Id = id });

                    await connection.CloseAsync();
                }
            }
            catch (CommissionNotFoundException)
            {
                // Logging
                Console.WriteLine($"CommissionNotFoundException in DeleteCommissionAsync");
                throw;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in DeleteCommissionAsync: {ex.Message}");
                throw;
            }
        }

        public bool IsUserAuthorized(string userEmail, Guid commissionId)
        {
            try
            {
                Commission existingCommission = GetCommissionByIdAsync(commissionId).Result;

                //Check if the existing commission's email matches the user's email
                return existingCommission != null && existingCommission.email == userEmail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in IsUserAuthorized: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Commission>> GetRecentCommissionsAsync(string email)
        {
            try
            {
                string sql = "SELECT TOP 4 * " +
                             "FROM Commissions " +
                             "WHERE email = @Email " +
                             "ORDER BY " +
                             "CASE " +
                             "WHEN status = 'Completed' THEN 0 " +
                             "WHEN status = 'Queued' THEN 1 " +
                             "WHEN status = 'In Progress' THEN 2 " +
                             "ELSE 3 " +
                             "END, CommissionedDate DESC";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                List<Commission> recentCommissions = new List<Commission>();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    recentCommissions = (await connection.QueryAsync<Commission>(sql, new { Email = email })).ToList();

                    await connection.CloseAsync();
                }

                return recentCommissions;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in GetRecentCommissionsByEmailAsync: {ex.Message}");
                throw;
            }
        }

    }
}



