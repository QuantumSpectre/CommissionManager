namespace CommissionManagerAPP.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Password { get; set; }

        public UserProfile()
        {
            if (CreatedDate == null)
            {
                CreatedDate = DateTime.Now;
            }

            Id = Guid.NewGuid();
        }
    }
}
