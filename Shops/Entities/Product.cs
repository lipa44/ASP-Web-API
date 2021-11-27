using System;
using Shops.Tools;

namespace Shops.Entities
{
    public class Product
    {
        public Product(string name, int basePrice)
        {
            if (basePrice <= 0) throw new ShopException("Product base price must be positive", new ArgumentException());

            Id = Guid.NewGuid();
            Name = name;
            BasePrice = basePrice;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int BasePrice { get; }
    }
}