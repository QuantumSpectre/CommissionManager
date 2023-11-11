using System.Collections.Generic;

namespace CommissionManager.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public List<Commission> Commissions { get; set; }
        public string? name { get; set; }
        public UserProfile UserProfile { get; set; }

        public Artist()
        {
            Commissions = new List<Commission>();
        }
    }
}
