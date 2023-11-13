using CommissionManager.API.Exceptions;
using CommissionManager.API.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CommissionManager.API.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IConfiguration _configuration;

        public ArtistRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Artist>> GetArtistsAsync()
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

                return artists;
            }
            catch (Exception ex)
            {
                // Logging
                throw;
            }
        }

        public async Task<Artist> GetArtistByIdAsync(Guid id)
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
                        throw new ArtistNotFoundException($"Artist with ID {id} not found");
                    }
                }

                return artist;
            }
            catch (Exception ex)
            {
                // Logging
                throw;
            }
        }

        public async Task<Guid> CreateArtistAsync(Artist artist)
        {
            try
            {
                //Generates the GUID
                Guid artistId = Guid.NewGuid();

                string sql = @"
            INSERT INTO Artists (Id, Name)
            Values (@Id, @Name);
        ";

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(sql, new { Id = artistId, artist.Name });

                    await connection.CloseAsync();

                    return artistId;
                }
            }
            catch (Exception ex)
            {
                // Logging
                throw;
            }
        }

        public async Task UpdateArtistAsync(Guid id, Artist updatedArtist)
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
                        throw new ArtistNotFoundException($"Artist with ID {id} not found");
                    }

                    string updateSql = @"
                UPDATE Artists
                SET Name = @Name
                WHERE Id = @Id;
            ";

                    await connection.ExecuteAsync(updateSql, updatedArtist);

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                // Logging
                throw;
            }
        }

        public async Task DeleteArtistAsync(Guid id)
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
                        throw new ArtistNotFoundException($"Artist with ID {id} not found");
                    }

                    string deleteSql = "DELETE FROM Artists WHERE Id = @Id";
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

