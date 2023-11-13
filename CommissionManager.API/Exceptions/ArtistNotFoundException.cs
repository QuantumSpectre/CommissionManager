namespace CommissionManager.API.Exceptions
{
    public class ArtistNotFoundException : Exception
    {
        public ArtistNotFoundException()
        {
        }

        public ArtistNotFoundException(string message)
            : base(message)
        {
        }

        public ArtistNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
