using CommissionManagerAPP.Models;
using CommissionManagerAPP.Repositories;

namespace CommissionManagerAPP.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _userRepository;

        public UserProfileService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfile> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userRepository.GetUserProfileByEmailAsync(email);
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Invalid parameter provided.", ex);
            }
        }

        public async Task<bool> RegisterAsync(UserProfile userProfile)
        {
            try
            {
                //Hash password and assign property
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userProfile.Password);
                userProfile.Password = hashedPassword;

                //Save user to db
                await _userRepository.SaveUserProfileAsync(userProfile);

                return true;
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Invalid parameter provided.", ex);
            }
        }

        public async Task<bool> UpdatePasswordAsync(string email, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetUserProfileByEmailAsync(email);

                if (user != null)
                {                    
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    user.Password = hashedPassword;

                    await _userRepository.SaveUserProfileAsync(user);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Invalid parameter provided.", ex);
            }
        }

        public async Task<bool> VerifyPasswordAsync(string email, string enteredPassword)
        {
            try
            {
                var user = await _userRepository.GetUserProfileByEmailAsync(email);

                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(enteredPassword, user.Password))
                    {
                        return true;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Invalid parameter provided.", ex);
            }

            return false;
        }
    }
}