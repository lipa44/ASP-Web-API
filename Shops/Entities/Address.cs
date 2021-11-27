using System;
using Shops.Tools;

namespace Shops.Entities
{
    public class Address
    {
        public Address(string street, int houseNumber)
        {
            if (string.IsNullOrEmpty(street)) throw new ShopException("Street name is null", new ArgumentException());
            if (houseNumber <= 0) throw new ShopException("House number must be positive", new ArgumentException());

            Name = $"{street}, {houseNumber}";
        }

        public string Name { get;  }
    }
}