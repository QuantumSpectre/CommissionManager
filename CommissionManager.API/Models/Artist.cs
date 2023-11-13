namespace CommissionManager.API.Models
{
    public class Artist
    {
        public Guid Id { get; set; }
        public List<Commission> Commissions { get; set; }
        public string Name { get; set; }        

        public Artist()
        {
            Commissions = new List<Commission>();
        }
    }
}
