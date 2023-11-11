using CommissionManager.API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CommissionManager.API.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ArtistsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistById(int id)
        {
            try
            {
                Artist artist;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    artist = await connection.QuerySingleOrDefaultAsync<Artist>("SELECT * FROM Artists WHERE Id = @Id", new { Id = id });

                    if (artist == null)
                    {
                        return NotFound();
                    }
                }

                return Ok(artist);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetArtistsAsync()
        {
            try
            {
                string sql = "SELECT * FROM Artists";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                List<Artist> artists = new List<Artist>();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    artists = (await connection.QueryAsync<Artist>(sql)).ToList();

                    await connection.CloseAsync();
                }

                return Ok(artists);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtistAsync([FromBody] Artist artist)
        {
            try
            {
                string sql = @"
                    INSERT INTO Artists (Name)
                    Values (@Name);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    ";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    int newArtistId = await connection.QuerySingleAsync<int>(sql, artist);

                    return Ok(newArtistId);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateArtistAsync(int id, [FromBody] Artist updatedArtist)
        {
            try
            {
                if (updatedArtist == null)
                {
                    return BadRequest("Updated artist data is null.");
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var existingArtist = await connection.QuerySingleOrDefaultAsync<Artist>("SELECT * FROM Artists WHERE Id = @Id", new { Id = id });

                    if (existingArtist == null)
                    {
                        return NotFound();
                    }

                    string updateSql = @"
                        UPDATE Artists
                        SET Name = @Name
                        WHERE Id = @Id;
                    ";

                    await connection.ExecuteAsync(updateSql, updatedArtist);

                    await connection.CloseAsync();
                    return Ok(updatedArtist);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var existingArtist = await connection.QuerySingleOrDefaultAsync<Artist>("SELECT * FROM Artists WHERE Id = @Id", new { Id = id });

                    if (existingArtist == null)
                    {
                        return NotFound();
                    }

                    string deleteSql = "DELETE FROM Artists WHERE Id = @Id";
                    await connection.ExecuteAsync(deleteSql, new { Id = id });

                    await connection.CloseAsync();
                    return Ok(new { message = "Artist deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

