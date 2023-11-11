using System.Collections.Generic;

namespace CommissionManager.API.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Commission> Commissions { get; set; }

        public Client()
        {
            Commissions = new List<Commission>();
        }
    }
}
