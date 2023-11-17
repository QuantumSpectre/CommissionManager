using CommissionManagerAPP.Models;

namespace CommissionManagerAPP.Repositories
{
    public interface ICommissionRepository
    {
        Task<Commission> GetCommissionByIdAsync(Guid id);
        Task<List<Commission>> GetCommissionsAsync();
        Task<Guid> CreateCommissionAsync(Commission commission);
        Task UpdateCommissionAsync(Guid id,Commission updatedcommission);
        Task DeleteCommissionAsync(Guid id);
    }
}
