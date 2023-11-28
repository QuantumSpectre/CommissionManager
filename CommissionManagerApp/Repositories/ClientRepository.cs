using CommissionManagerAPP.Exceptions;
using CommissionManagerAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace CommissionManagerAPP.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IConfiguration _configuration;

        public ClientRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            try
            {
                string sql = "SELECT * FROM Clients";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                List<Client> clients = new List<Client>();

                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    clients = (await connection.QueryAsync<Client>(sql)).ToList();

                    await connection.CloseAsync();
                }

                return clients;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in GetClientsAsync: {ex.Message}");

                throw;
            }
        }

        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            try
            {
                Client client;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    client = await connection.QuerySingleOrDefaultAsync<Client>("SELECT * FROM Clients WHERE Id = @Id", new { Id = id });

                    if (client == null)
                    {
                        throw new ClientNotFoundException($"Client with ID {id} not found");
                    }
                }

                return client;
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in GetClientByIdAsync: {ex.Message}");

                throw;
            }
        }

        public async Task<Guid> CreateClientAsync(Client client)
        {
            try
            {
                Guid clientId = Guid.NewGuid();

                string sql = @"
            INSERT INTO Clients (Id, Name)
            VALUES (@Id, @Name);
        ";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(sql, new { Id = clientId, client.Name });

                    await connection.CloseAsync();

                    return clientId;
                }
            }
            catch (Exception ex)
            {
                // Logging
                Console.WriteLine($"An error occurred in CreateClientAsync: {ex.Message}");

                throw;
            }
        }


        public async Task UpdateClientAsync(Guid id, Client updatedClient)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var existingClient = await connection.QuerySingleOrDefaultAsync<Client>("SELECT * FROM Clients WHERE Id = @Id", new { Id = id });

                    if (existingClient == null)
                    {
                        throw new ClientNotFoundException($"Client with ID {id} not found");
                    }

                    string updateSql = @"
                        UPDATE Clients
                        SET Name = @Name
                        WHERE Id = @Id;
                    ";

                    await connection.ExecuteAsync(updateSql, updatedClient);

                    await connection.CloseAsync();
                }
            }
            catch (ClientNotFoundException)
            {
                // Logging
                Console.WriteLine($"ClientNotFoundException in UpdateClientAsync");

                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in UpdateClientAsync: {ex.Message}");

                throw;
            }
        }

        public async Task DeleteClientAsync(Guid id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var existingClient = await connection.QuerySingleOrDefaultAsync<Client>("SELECT * FROM Clients WHERE Id = @Id", new { Id = id });

                    if (existingClient == null)
                    {
                        throw new ClientNotFoundException($"Client with ID {id} not found");
                    }

                    string deleteSql = "DELETE FROM Clients WHERE Id = @Id";

                    await connection.ExecuteAsync(deleteSql, new { Id = id });

                    await connection.CloseAsync();
                }
            }
            catch (ClientNotFoundException)
            {
                // Log the specific exception and handle accordingly
                Console.WriteLine($"ClientNotFoundException in DeleteClientAsync");
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred in DeleteClientAsync: {ex.Message}");
                throw;
            }
        }
    }
}
