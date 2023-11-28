using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.GUI
{
    public static class ApiEndpoints
    {
        //Make sure this isn't https, that gives an error everytime
        public const string BaseUrl = "http://localhost:5000"; 
        public const string Users = BaseUrl + "/api/Users";
        public const string VerifyPassword = BaseUrl + "/api/Users/VerifyPassword";
        public const string Commissions = BaseUrl + "/api/Commissions";
        public const string Clients = BaseUrl + "/api/Clients";
    }
}
