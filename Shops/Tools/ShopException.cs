using System;

namespace Shops.Tools
{
    public class ShopException : Exception
    {
        public ShopException()
        {
        }

        public ShopException(string message)
            : base(message)
        {
        }

        public ShopException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}