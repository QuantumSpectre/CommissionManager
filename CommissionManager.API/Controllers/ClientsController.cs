using CommissionManager.API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CommissionManager.API.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClientsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
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
                        return NotFound();
                    }
                }

                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsAsync()
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

                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientAsync([FromBody] Client client)
        {
            try
            {
                string sql = @"
                    INSERT INTO Clients (Name)
                    Values (@Name);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    ";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    int newClientId = await connection.QuerySingleAsync<int>(sql, client);

                    return Ok(newClientId);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateClientAsync(int id, [FromBody] Client updatedClient)
        {
            try
            {
                if (updatedClient == null)
                {
                    return BadRequest("Updated Client data is null.");
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var existingClient = await connection.QuerySingleOrDefaultAsync<Client>("SELECT * FROM Clients WHERE Id = @Id", new { Id = id });

                    if (existingClient == null)
                    {
                        return NotFound();
                    }

                    string updateSql = @"
                        UPDATE Clients
                        SET Name = @Name
                        WHERE Id = @Id;
                    ";

                    await connection.ExecuteAsync(updateSql, updatedClient);

                    await connection.CloseAsync();
                    return Ok(updatedClient);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
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
                        return NotFound();
                    }

                    string deleteSql = "DELETE FROM Clients WHERE Id = @Id";
                    await connection.ExecuteAsync(deleteSql, new { Id = id });

                    await connection.CloseAsync();
                    return Ok(new { message = "Client deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
