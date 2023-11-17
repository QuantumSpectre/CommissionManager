using CommissionManagerAPP.Exceptions;
using CommissionManagerAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CommissionManagerAPP.Repositories
{
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
                        throw new CommissionNotFoundException($"Commission with ID {id} not found");
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
            INSERT INTO Commissions (Id, Description, ClientID, CommissionedDate, Deadline, Status)
            VALUES (@Id, @Description, @ClientId, @CommissionedDate, @Deadline, @Status);";

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
                        commission.status
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
    }
}



