using System;
using System.ComponentModel.DataAnnotations;

namespace CommissionManager.GUI.Models
{
    public class Commission
    {
        public Guid Id {  get; set; }
        public Guid ClientId { get; set; }
        public DateTime CommissionedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        public Commission()
        {
            CommissionedDate = DateTime.Now;
            Deadline = DateTime.Now.AddDays(14);
            Status = "Queued";
        }
    }
}
