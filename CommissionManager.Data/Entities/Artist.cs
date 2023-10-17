using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data.Entities
{
    public class Artist
    {
        public int ArtistId { get; set; }

        public List<Commission> Commissions { get; set; }

        public string? Name { get; set; }
        
        public Artist()
        {
            Commissions = new List<Commission>();
        }

        public Artist(string name) : this()
        {
            Name = name;   
        }
    }
}
