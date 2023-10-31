using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.BusinessLogic.Domain
{
    public class Commission
    {
        public Artist Artist { get; set; }
        public DateTime CommissionedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public String? Description { get; set; }
        public int CommissionId { get; set; }
        public CommissionStatus Status { get; set; }

        public Commission()
        {
            CommissionedDate = DateTime.Now;
            Status = CommissionStatus.Queued;
            Deadline = CommissionedDate.AddDays(14);
        }
    }
}
