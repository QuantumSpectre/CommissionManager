using CommissionManager.API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CommissionManager.API.Controllers
{
    //AS per the SOLID principles every method is responsible for a specific action such as Creating updating deleting and retrieving.
    //Each method is closed for modification as if we want more functionality added we can do so without modifying the existing code.
    //Because the code uses abstractions rather than concrete implementations its also adherant to the Dependancy inversion principle.


    [Route("api/commissions")]
    [ApiController]
    public class CommissionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        //obtain config to get connection stream
        public CommissionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        //get specific com by ID property
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommissionById(int id)
        {
            try
            {
                Commission commission;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Retrieve the record by Id
                    commission = await connection.QuerySingleOrDefaultAsync<Commission>("SELECT * FROM Commissions WHERE Id = @Id", new { Id = id });

                    // Return null if it doesn't exist
                    if (commission == null)
                    {
                        return NotFound();
                    }
                } // The using statement will automatically close the connection

                return Ok(commission); // 200 OK with the commission data
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //Get all those commissions 
        [HttpGet]
        public async Task<IActionResult> GetCommissionsAsync()
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

                return Ok(commissions);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateCommissionAsync([FromBody] Commission commission)
        {
            try
            {
                string sql = @"
                    INSERT INTO Commissions (Description, ArtistId, ClientID, CommissionedDate, Deadline, Status)
                    Values (@Description, @ArtistID, @ClientId, @CommissionedDate, @Deadline, @Status);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    ";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    int newCommissionId = await connection.QuerySingleAsync<int>(sql, commission);

                    return Ok(newCommissionId);
                }


            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCommissionAsync(int id, [FromBody] Commission updatedCommission)
        {
            try
            {
                //Checks to see if it exists
                if (updatedCommission == null)
                {
                    return BadRequest("Updated commission data is null.");
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                //wraps everything in a using connection statement
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    //retreives the record in question using the URI
                    var existingCommission = await connection.QuerySingleOrDefaultAsync<Commission>("SELECT * FROM Commissions WHERE Id = @Id", new { Id = id });
                    //Another validation of its existence
                    if (existingCommission == null)
                    {
                        return NotFound();
                    }

                    //plug in the new values from from a json
                    string updateSql = @"
                UPDATE Commissions
                SET Description = @Description,
                    ArtistId = @ArtistId,
                    ClientId = @ClientId,
                    CommissionedDate = @CommissionedDate,
                    Deadline = @Deadline,
                    Status = @Status
                WHERE Id = @Id;
            ";

                    await connection.ExecuteAsync(updateSql, updatedCommission);

                    await connection.CloseAsync();
                    return Ok(updatedCommission);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommission(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    //open the connection
                    await connection.OpenAsync();

                    //Retrieve the record we wish to delete
                    var existingCommission = await connection.QuerySingleOrDefaultAsync<Commission>("SELECT * FROM Commissions WHERE Id = @Id", new { Id = id });
                    //return null if it doesn't exist
                    if (existingCommission == null)
                    {
                        return NotFound();
                    }

                    // Delete the record
                    string deleteSql = "DELETE FROM Commissions WHERE Id = @Id";
                    await connection.ExecuteAsync(deleteSql, new { Id = id });
                    //Explicitly closing connection
                    await connection.CloseAsync();
                    return Ok(new { message = "Commission deleted successfully " });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
