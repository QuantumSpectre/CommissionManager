using CommissionManagerAPP.Models;

namespace CommissionManagerAPP.Services
{
    public interface ICommissionService 
    {
        Task<List<Commission>> GetRecentCommissionsAsync(string email);
        Task<List<Commission>> GetCommissionsByEmailAsync(string email);
    }
}
