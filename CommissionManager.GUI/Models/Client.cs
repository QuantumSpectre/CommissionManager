using System.Collections.Generic;

namespace CommissionManager.GUI.Models
{
    public class Client
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Commission> Commissions { get; set; }

        public Client()
        {
            Commissions = new List<Commission>();
        }
    }
}
