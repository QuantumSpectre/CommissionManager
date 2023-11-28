using CommissionManagerAPP.Models;

namespace CommissionManagerAPP.Services
{
    public interface IUserProfileService
    {
        //Handling Regestration
        Task<bool> RegisterAsync(UserProfile userProfile);
        //Password operations
        Task<bool> UpdatePasswordAsync(string email, string newPassword);
        Task<bool> VerifyPasswordAsync(string email, string enteredPassword);
        //Retreiving the user
        Task<UserProfile> GetUserByEmailAsync(String userId);
    }
}
