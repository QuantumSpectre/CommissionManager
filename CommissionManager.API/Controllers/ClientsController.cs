using CommissionManager.API.Models;
using CommissionManager.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace CommissionManager.API.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;

        public ClientsController(IConfiguration configuration, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            try
            {
                Client client = await _clientRepository.GetClientByIdAsync(id);
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
                var clients = await _clientRepository.GetClientsAsync();
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
                Guid newClientId = await _clientRepository.CreateClientAsync(client);
                return Ok(newClientId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateClientAsync(Guid id, [FromBody] Client updatedClient)
        {
            try
            {
                await _clientRepository.UpdateClientAsync(id, updatedClient);
                return Ok(updatedClient);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            try
            {
                await _clientRepository.DeleteClientAsync(id);
                return Ok(new { message = "Client deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
