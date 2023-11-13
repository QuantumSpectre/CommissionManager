using System.Collections.Generic;

namespace CommissionManager.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public List<Commission> Commissions { get; set; }
        public string? Name { get; set; }
        

        public Artist()
        {
            Commissions = new List<Commission>();
        }
    }
}
