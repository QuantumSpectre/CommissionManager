using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data.Entities
{
    public class ArtistEntity
    {
        public int ArtistId { get; set; }

        public List<CommissionEntity> Commissions { get; set; }

        public string? Name { get; set; }
        
        public ArtistEntity()
        {
            Commissions = new List<CommissionEntity>();
        }

        public ArtistEntity(string name) : this()
        {
            Name = name;   
        }
    }
}
