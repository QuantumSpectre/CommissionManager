using CommissionManager.API.Models;

namespace CommissionManager.API.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> GetArtistByIdAsync(Guid id);
        Task<List<Artist>> GetArtistsAsync();
        Task<Guid> CreateArtistAsync(Artist artist);
        Task UpdateArtistAsync(Guid id, Artist updatedArtist);
        Task DeleteArtistAsync(Guid id);
        
        
    }
}
