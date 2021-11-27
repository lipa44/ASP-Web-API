using System;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Entities
{
    public class ProductOrder
    {
        public ProductOrder(Product product, int amount)
        {
            Product = product ?? throw new ShopException("Product is null", new ArgumentException());
            Amount = amount > 0 ? amount : throw new ShopException("Amount is null", new ArgumentException());
        }

        public Product Product { get; }
        public int Amount { get; private set; }

        public void IncreaseAmount(int amount = 1)
        {
            if (amount <= 0) throw new ShopException("Amount to change must be positive", new ArgumentException());
            Amount += amount;
        }
    }
}