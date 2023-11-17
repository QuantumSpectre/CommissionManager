using System;

namespace CommissionManagerAPP.Models
{
    public class Commission
    {
        public DateTime CommissionedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public string? Description { get; set; }
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public String status { get; set; }

        public Commission()
        {
            
            CommissionedDate = DateTime.Now;
            Deadline = DateTime.Now.AddDays(14);
           
        }
    }
}
