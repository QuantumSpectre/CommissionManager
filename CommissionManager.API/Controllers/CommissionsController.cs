using CommissionManager.API.Models;
using CommissionManager.API.Repositories;
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
        private readonly ICommissionRepository _commissionRepository;

        //obtain config to get connection stream
        public CommissionsController(IConfiguration configuration, ICommissionRepository commissionRepository)
        {
            _configuration = configuration;
            _commissionRepository = commissionRepository;
        }
        
        //get specific com by ID property
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommissionById(Guid id)
        {
            try
            {
                Commission commission = await _commissionRepository.GetCommissionByIdAsync(id);
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
               var commissions = await _commissionRepository.GetCommissionsAsync();

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
               Guid newCommissionId = await _commissionRepository.CreateCommissionAsync(commission);

                return Ok(newCommissionId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCommissionAsync(Guid id, [FromBody] Commission updatedCommission)
        {
            try
            {
                await _commissionRepository.UpdateCommissionAsync(id, updatedCommission);
                return Ok(updatedCommission);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommission(Guid id)
        {
            try
            {
                await _commissionRepository.DeleteCommissionAsync(id);
                return Ok(new { message = "Commission deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
