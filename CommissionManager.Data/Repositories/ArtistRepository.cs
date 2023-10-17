using CommissionManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        public readonly AppDbContext _context;

        public ArtistRepository(AppDbContext context)
        {
            _context = context;
        }

        //CRUD 

        //Retrieve
        public Artist GetArtistById(int artistId)
        {
            return _context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
        }

        public List<Artist> GetAllArtists()
        {
            return _context.Artists.ToList();
        }

        //Create
        public void AddArtist(Artist artist)
        {
            _context.Artists.Add(artist);
            _context.SaveChanges();
        }

        //Update
        public void UpdateArtist(Artist artist)
        {
            _context.Entry(artist).State = EntityState.Modified;
            _context.SaveChanges();
        }

        //Delete
        public void DeleteArtist(int artistId)
        {
            var artistToDelete = _context.Artists.Find(artistId);
            if (artistToDelete != null)
            {
                _context.Artists.Remove(artistToDelete);
                _context.SaveChanges();
            }
        }


    }
}
