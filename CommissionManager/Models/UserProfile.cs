using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Models
{
    public class UserProfile
    {
        public Artist Artist { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

        public UserProfile()
        {
                CreatedDate = DateTime.Now;
        }
    }
}
