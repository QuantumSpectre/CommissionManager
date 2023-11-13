using CommissionManager.API.Models;

namespace CommissionManager.API.Repositories
{
    public interface IClientRepository
    {

        Task<Client> GetClientByIdAsync(Guid id);
        Task<List<Client>> GetClientsAsync();
        Task<Guid> CreateClientAsync(Client client);
        Task UpdateClientAsync(Guid id, Client updatedClient);
        Task DeleteClientAsync(Guid id);

    }
}
