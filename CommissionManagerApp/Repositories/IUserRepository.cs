using CommissionManagerAPP.Models;

namespace CommissionManagerAPP.Repositories
{
    //Interface Secregation Principle
    public interface IUserRepository
    {
        Task<bool> SaveUserProfileAsync(UserProfile userProfile);
        Task<UserProfile> GetUserProfileByEmailAsync(String email);
        Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile);
    }
}
