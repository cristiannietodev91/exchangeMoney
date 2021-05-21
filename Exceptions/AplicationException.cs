using System;
namespace moneyExchange.Exceptions
{
    public class AplicationException: Exception
    {
        public AplicationException()
        {
        }

        public AplicationException(string message)
        : base(message) { }

        public AplicationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
