#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        public Customer(string name, int money)
        {
            if (string.IsNullOrEmpty(name)) throw new ShopException("Name is null", new ArgumentException());
            if (money <= 0)
                throw new ShopException("Amount of money must be positive", new ArgumentException());

            Name = name;
            Money = money;
            ProductBag = new List<ProductOrder>();
        }

        public int Money { get; private set; }
        public string Name { get; }
        public List<ProductOrder> ProductBag { get; }

        public bool AbleToBuyProduct(int price)
        {
            if (price <= 0) throw new ShopException("Price of product to buy must be positive", new ArgumentException());
            return Money < price;
        }

        public void AddMoney(int money)
        {
            if (money <= 0) throw new ShopException("Money amount must be positive", new ArgumentException());

            Money += money;
        }

        public void BuyProduct(Product shopProduct, int price)
        {
            if (shopProduct is null) throw new ShopException("Shop product is null", new ArgumentException());
            if (price <= 0) throw new ShopException("Price must be positive", new ArgumentException());

            Money -= price;

            ProductOrder? boughtProduct = ProductBag.SingleOrDefault(p => p.Product.Id == shopProduct.Id);

            if (boughtProduct is null) ProductBag.Add(new ProductOrder(shopProduct, 1));
            else boughtProduct.IncreaseAmount();
        }
    }
}