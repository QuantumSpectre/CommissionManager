using CommissionManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data.Repositories
{
    public interface IArtistRepository
    {
        Artist GetArtistById(int artistId);
        List<Artist> GetAllArtists();
        void AddArtist(Artist artist);
        void UpdateArtist(Artist artist);
        void DeleteArtist(int artistId);
    }
}
