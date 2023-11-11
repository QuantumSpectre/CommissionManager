using System;

namespace CommissionManager.API.Models
{
    public class Commission
    {
        
        public int ArtistId { get; set; }
        
        public DateTime CommissionedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public string? Description { get; set; }
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public String status { get; set; }

        public Commission()
        {
            
            CommissionedDate = DateTime.Now;
            Deadline = DateTime.Now.AddDays(14);
           
        }
    }
}
