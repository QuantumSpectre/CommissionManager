using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.GUI.Models
{
    public class AuthToken
    {
        public bool Authenticated {  get; set; }
        public string? Email {  get; set; }

        public AuthToken() { }

        public AuthToken(string _email, bool _authenticated)
        {
            this.Email = _email;
            this.Authenticated = _authenticated;
        }

        public void SetAuthToken(string email)
        {
            Email = email;
            Authenticated = true;
        }
    }
}
