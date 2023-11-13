using CommissionManager.API.Exceptions;
using CommissionManager.API.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CommissionManager.API.Repositories
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

                using (var connection = new SqlConnection(connectionString))
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
                throw;
            }
        }

        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            try
            {
                Client client;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
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

                using (var connection = new SqlConnection(connectionString))
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
                throw;
            }
        }


        public async Task UpdateClientAsync(Guid id, Client updatedClient)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
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
            catch (Exception ex)
            {
                // Logging
                throw;
            }
        }

        public async Task DeleteClientAsync(Guid id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
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
            catch (Exception ex)
            {
                // Logging
                throw;
            }
        }
    }
}
