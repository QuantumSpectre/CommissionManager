using System.Collections.Generic;

namespace CommissionManagerAPP.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Commission> Commissions { get; set; }

        public Client()
        {
            Commissions = new List<Commission>();
        }
    }
}
