using System;

namespace CommissionManagerAPP.Exceptions
{
    public class CommissionNotFoundException : Exception
    {
        public CommissionNotFoundException()
        {
        }

        public CommissionNotFoundException(string message) : base(message)
        {
        }

        public CommissionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}