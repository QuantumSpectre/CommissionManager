using CommissionManagerAPP.Models;

namespace CommissionManagerAPP.Repositories
{
    public interface IUserRepository
    {
        Task<bool> SaveUserProfileAsync(UserProfile userProfile);
        Task<UserProfile> GetUserProfileByEmailAsync(String email);
        Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile);
    }
}
