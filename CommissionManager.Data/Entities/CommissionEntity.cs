using CommissionManager.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data.Entities
{
    public class CommissionEntity
    {

        public ArtistEntity? Artist { get; set; }

        public int ArtistId { get; set; }
        public DateTime CommissionedDate { get; set; }
        public DateTime Deadline { get; set; }
        public string? Description { get; set; }
        public int CommissionId { get; set; }
        public int CommissionStatusId { get; set; }
        public CommissionStatus Status { get; set; }

        public CommissionEntity()
        {
            CommissionedDate = DateTime.Now;
            Status = CommissionStatus.Queued;
            Deadline = CommissionedDate.AddDays(14);
        }

    }
}
