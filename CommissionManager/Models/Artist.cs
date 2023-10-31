using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Models
{
    class Artist
    {
        public int ArtistId { get; set; }
        public List<Commission> Commissions { get; set; }
        public string? name { get; set; }

        public Artist()
        {
            Commissions = new List<Commission>();
        }
    }
}
