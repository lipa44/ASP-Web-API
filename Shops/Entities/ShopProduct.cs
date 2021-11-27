using System;
using Shops.Tools;

namespace Shops.Entities
{
    public class ShopProduct
    {
        public ShopProduct(Product product, int price, int amount)
        {
            if (price <= 0) throw new ShopException("Product price must be positive", new ArgumentException());
            if (amount <= 0) throw new ShopException("Product amount must be positive", new ArgumentException());

            Price = price;
            Amount = amount;
            Product = product ?? throw new ShopException("Product is null", new ArgumentException());
        }

        public int Price { get; private set; }
        public int Amount { get; private set; }
        public Product Product { get; }

        public void ChangePrice(int newPrice)
        {
            if (newPrice <= 0) throw new ShopException("Product's price must be positive", new ArgumentException());

            Price = newPrice;
        }

        public void DecreaseAmount(int amount)
        {
            if (amount <= 0) throw new ShopException("Amount to decrease must be positive", new ArgumentException());

            if (Amount - amount < 0)
                throw new ShopException($"Not enough product {Product.Name} to buy", new ArgumentException());

            Amount -= amount;
        }

        public void IncreaseAmount(int amount)
        {
            if (amount <= 0) throw new ShopException("Amount to increase must be positive", new ArgumentException());

            Amount += amount;
        }
    }
}