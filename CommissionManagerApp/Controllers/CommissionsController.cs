using CommissionManager.GUI.Models;
using CommissionManagerAPP.Models;
using CommissionManagerAPP.Repositories;
using CommissionManagerAPP.Services;
using Microsoft.AspNetCore.Mvc;

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
        private readonly ICommissionService _commissionService;

        //obtain config to get connection stream
        public CommissionsController(IConfiguration configuration, ICommissionRepository commissionRepository, ICommissionService commissionService)
        {
            _configuration = configuration;
            _commissionRepository = commissionRepository;
            _commissionService = commissionService;
        }

        //get specific com by email property
        [HttpGet("byemail/{email}")]
        public async Task<IActionResult> GetCommissionByEmail([FromRoute] string email)
        {
            try
            {
               List<Commission> commission = await _commissionService.GetCommissionsByEmailAsync(email);

                return Ok(commission); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCommissionByIdAsync(Guid id)
        {
            try
            {
                Commission commission = await _commissionRepository.GetCommissionByIdAsync(id);

                if (commission == null)
                {
                    return NotFound($"Commission with ID {id} not found");
                }

                return Ok(commission);
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
        public async Task<IActionResult> UpdateCommissionAsync(Guid id, [FromBody] Commission updatedCommission, [FromQuery] string email)
        {
            try
            {
                // Check email for authorization
                //I'm certain i can return the object if they pass in this code but for simplicity lets not
                if (!_commissionRepository.IsUserAuthorized(email, id))
                {
                    return Unauthorized("Unauthorized: User is not allowed to update this commission.");
                }
                //Call update method
                await _commissionRepository.UpdateCommissionAsync(id, updatedCommission);

                return Ok(updatedCommission);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommission(Guid id, [FromQuery] string email)
        {
            if (!_commissionRepository.IsUserAuthorized(email, id))
            {
                return Unauthorized("Unauthorized: User is not allowed to update this commission.");
            }

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

        [HttpGet("recent/{email}")]
        public async Task<IActionResult> GetRecentCommissionsAsync([FromRoute] string email)
        {
            try
            {
                var recentCommissions = await _commissionService.GetRecentCommissionsAsync(email);

                return Ok(recentCommissions);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
