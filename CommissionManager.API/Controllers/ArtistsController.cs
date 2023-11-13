using CommissionManager.API.Models;
using CommissionManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace CommissionManager.API.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;

        private readonly IConfiguration _configuration;

        public ArtistsController(IConfiguration configuration, IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistById(Guid id)
        {
            try
            {
                Artist artist = await _artistRepository.GetArtistByIdAsync(id);
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
                var artists = await _artistRepository.GetArtistsAsync();
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
                Guid newArtistId = await _artistRepository.CreateArtistAsync(artist);
                return Ok(newArtistId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateArtistAsync(Guid id, [FromBody] Artist updatedArtist)
        {
            try
            {
                await _artistRepository.UpdateArtistAsync(id, updatedArtist);
                return Ok(updatedArtist);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(Guid id)
        {
            try
            {
                await _artistRepository.DeleteArtistAsync(id);
                return Ok(new { message = "Artist deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

