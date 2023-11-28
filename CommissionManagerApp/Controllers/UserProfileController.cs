using CommissionManagerAPP.Models;
using CommissionManagerAPP.Repositories;
using CommissionManagerAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommissionManagerAPP.Controllers
{
    //Open Closed Principle responsible for specifically CRUD operations. Closed for moding, allowing for extension.
    //Dependency Inversion Principle - depends on abstractions rather than concrete implementations
    [Route("api/users")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IConfiguration configuration, IUserRepository userRepository, IUserProfileService userProfileService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userProfileService = userProfileService;
        }

        [HttpPost("VerifyPassword")]
        public async Task<IActionResult> VerifyPasswordAsync( [FromBody] UserAuthenticationRequest userRequest)
        {
            try
            {
                bool passwordVerified = await _userProfileService.VerifyPasswordAsync(userRequest.Email, userRequest.Password);

                if (passwordVerified)
                {
                    return Ok("Password verified successfully");
                }
                else
                {
                    return NotFound("User with the combination of email and password not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in VerifyPasswordAsync: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmailAsync([FromQuery] string email)
        {
            try
            {
                UserProfile user = await _userRepository.GetUserProfileByEmailAsync(email);

                if (user == null)
                {
                    return NotFound(); 
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in the GetUserProfileByEmailAsync: {ex.Message}");

                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> SaveUserProfileAsync([FromBody]UserProfile user)
        {
            try
            {
                await _userRepository.SaveUserProfileAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in the SaveUserProfileAsync: {ex.Message}");

                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfileAsync(UserProfile user)
        {
            try
            {
              var Results =  await _userProfileService.RegisterAsync(user);

              return Ok(Results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CreateUserProfileAsync: {ex.Message}");

                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
