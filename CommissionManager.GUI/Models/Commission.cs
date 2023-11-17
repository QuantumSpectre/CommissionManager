using System;

namespace CommissionManager.GUI.Models
{
    public class Commission
    {
        
        public int Id {  get; set; }
        public int ClientId { get; set; }
        public DateTime CommissionedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } 

        public Commission()
        {
            CommissionedDate = DateTime.Now;
            Deadline = DateTime.Now.AddDays(14);
            Status = "Queued";
        }
    }
}
