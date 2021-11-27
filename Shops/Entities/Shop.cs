using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        public Shop(string name, Address address, int money)
        {
            if (string.IsNullOrEmpty(name)) throw new ShopException("Shop name is null", new ArgumentException());
            if (money <= 0) throw new ShopException("Shop money must be poisitive", new ArgumentException());

            Name = name;
            ShopAddress = address;
            Money = money;
            ShopProducts = new List<ShopProduct>();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public string Name { get; }
        public int Money { get; private set; }
        public Address ShopAddress { get; }
        public List<ShopProduct> ShopProducts { get; }
        public List<Product> ProductsList => ShopProducts.Select(p => p.Product).ToList();

        public void ChangeProductPrice(Product product, int newPrice)
        {
            if (product is null) throw new ShopException("Product is null", new ArgumentException());

            ShopProduct pProduct = FindShopProduct(product);

            if (pProduct is null)
                throw new ShopException($"Product {product.Name} doesn't exists in shop {Name}", new ArgumentException());

            pProduct.ChangePrice(newPrice);
        }

        public Product AddProduct(Product product, int amount)
        {
            if (product is null) throw new ShopException("Product is null", new ArgumentException());
            if (amount < 0) throw new ShopException("Product amount must be positive", new ArgumentException());

            if (product.BasePrice * amount > Money)
                throw new ShopException($"Not enough money in shop {Name} to buy {product.Name}");

            Money -= product.BasePrice * amount;

            var newProduct = new ShopProduct(product, product.BasePrice, amount);

            ShopProduct shopProduct = FindShopProduct(product);

            if (shopProduct is null) ShopProducts.Add(newProduct);
            else shopProduct.IncreaseAmount(amount);

            return newProduct.Product;
        }

        public void BuyProducts(Customer customer, List<ProductOrder> products)
        {
            if (customer is null) throw new ShopException("Customer is null", new ArgumentException());

            Dictionary<ShopProduct, int> shopProducts = FindShopProducts(products);

            if (shopProducts is null) throw new ShopException($"No such products in shop {Name}");

            foreach (KeyValuePair<ShopProduct, int> productToBuy in shopProducts)
            {
                if (customer.AbleToBuyProduct(productToBuy.Key.Price * productToBuy.Value))
                    throw new ShopException($"Not enough {productToBuy.Key.Price - customer.Money} money to buy {productToBuy.Key.Product.Name}");

                customer.BuyProduct(productToBuy.Key.Product, productToBuy.Key.Price * productToBuy.Value);

                DecreaseAmountOfProduct(productToBuy.Key, productToBuy.Value);

                Money += productToBuy.Key.Price * productToBuy.Value;
            }
        }

        public Dictionary<ShopProduct, int> FindShopProducts(List<ProductOrder> products)
        {
            if (products is null) throw new ShopException("Product are null", new ArgumentException());

            var productsWithAmount = new Dictionary<ShopProduct, int>();

            foreach (ProductOrder productOrder in products)
            {
                ShopProduct shopProduct = FindShopProduct(productOrder.Product);

                if (shopProduct is null || productOrder.Amount > shopProduct.Amount) return new Dictionary<ShopProduct, int>();

                productsWithAmount.Add(shopProduct, productOrder.Amount);
            }

            return productsWithAmount;
        }

        public void BuyProduct(Customer customer, Product product) => BuyProducts(customer, new List<ProductOrder> { new (product, 1) });

        public int GetProductPrice(Product product)
        {
            if (product is null) throw new ShopException("Product is null", new ArgumentException());

            return ShopProducts.Single(p => p.Product.Id == product.Id).Price;
        }

        public int GetProductAmount(Product product)
        {
            if (product is null) throw new ShopException("Product is null", new ArgumentException());

            return ShopProducts.Single(p => p.Product.Id == product.Id).Amount;
        }

        private void DecreaseAmountOfProduct(ShopProduct shopProduct, int count)
        {
            if (shopProduct is null) throw new ShopException("Shop product is null", new ArgumentException());

            shopProduct.DecreaseAmount(count);
        }

        private ShopProduct FindShopProduct(Product product)
        {
            if (product is null) throw new ShopException("Product is null", new ArgumentException());

            return ShopProducts.SingleOrDefault(p => p.Product.Id == product.Id);
        }
    }
}