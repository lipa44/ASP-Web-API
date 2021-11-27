using System;

namespace Banks.Tools
{
    public class BankException : Exception
    {
        public BankException()
        {
        }

        public BankException(string message)
            : base(message)
        {
        }

        public BankException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}