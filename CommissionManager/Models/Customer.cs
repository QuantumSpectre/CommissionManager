using System.Collections.Generic;

namespace CommissionManager.Models
{
    public class Customer
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
