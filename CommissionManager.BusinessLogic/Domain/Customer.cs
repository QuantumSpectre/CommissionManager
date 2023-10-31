namespace CommissionManager.BusinessLogic.Domain
{
    public class Customer
    {
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public List<Commission> Commissions { get; set; } = new List<Commission>();


    }
}
