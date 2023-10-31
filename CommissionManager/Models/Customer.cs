using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Models
{
    class Customer
    {
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public List<Commission> Commissions { get; set; }

        public Customer()
        {
            Commissions = new List<Commission>();
        }
    }
}
