using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Models
{
    class Commission
    {
        public Artist Artist { get; set; }
        public DateTime CommissionedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public string? Description { get; set; }
        public int CommissionId { get; set; }
        public CommissionStatus Status { get; set; } 

        public Commission()
        {
            Artist = new Artist();
            CommissionedDate = DateTime.Now;
            Deadline = DateTime.Now.AddDays(14);
            Status = CommissionStatus.Queued;
        }
    }
}
