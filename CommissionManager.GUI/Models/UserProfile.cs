using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.GUI.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? password { get; set; }

        public UserProfile()
        {
            if (CreatedDate == null)
            {
                CreatedDate = DateTime.Now;
            }

            Id = new Guid();
        }
    }
}
