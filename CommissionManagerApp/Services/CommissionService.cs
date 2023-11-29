using CommissionManagerAPP.Models;
using CommissionManagerAPP.Repositories;

namespace CommissionManagerAPP.Services
{
    public class CommissionService : ICommissionService
    {
        private readonly ICommissionRepository _repository;

        public CommissionService(ICommissionRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Commission>> GetCommissionsByEmailAsync(string email)
        {
            return _repository.GetCommissionByEmailAsync(email);
        }

        public async Task<List<Commission>> GetRecentCommissionsAsync(string email)
        {
            try
            {
                var recentCommissions = await _repository.GetRecentCommissionsAsync(email);

                // Perform any additional logic or transformations if needed
                var transformedCommissions = recentCommissions.Select(c => new Commission
                {
                    // Map properties as needed
                    Id = c.Id,
                    CommissionedDate = c.CommissionedDate,
                    Deadline = c.Deadline,
                    Description = c.Description,
                    ClientId = c.ClientId,
                    Status = c.Status,
                    Email = c.Email
                    // Add more properties as needed
                }).ToList();

                return transformedCommissions;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                Console.WriteLine($"An error occurred in CommissionService: {ex.Message}");
                throw;
            }
        }
    }
}
