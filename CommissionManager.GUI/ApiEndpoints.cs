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
        public const string BaseUrl = "https://localhost:5000"; 
        public const string Users = BaseUrl + "/api/Users";
        public const string VerifyPassword = BaseUrl + "/api/Users/VerifyPassword";
    }
}
