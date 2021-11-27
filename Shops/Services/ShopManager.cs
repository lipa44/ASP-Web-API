using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopManager : IShopManager
    {
        private readonly List<Product> _products;
        private readonly List<Shop> _shops;

        public ShopManager()
        {
            _products = new List<Product>();
            _shops = new List<Shop>();
        }

        public Shop CreateShop(Shop shop)
        {
            if (shop is null) throw new ShopException("Shop is null", new ArgumentException());
            if (ShopExist(shop)) throw new ShopException($"Shop {shop.Name} is already registered");

            _shops.Add(shop);

            return shop;
        }

        public void RegisterProducts(List<Product> products)
        {
            if (products is null) throw new ShopException("Shop is null", new ArgumentException());

            foreach (Product product in products)
            {
                if (ProductExist(product)) throw new ShopException($"Product {product.Name} is already registered");

                _products.Add(product);
            }
        }

        public List<Product> AddProductsToShop(Shop shop, List<ProductOrder> products)
        {
            if (products is null) throw new ShopException("Products is null", new ArgumentException());

            if (shop is null) throw new ShopException("Shop is null", new ArgumentException());

            foreach (ProductOrder product in products)
            {
                if (!ProductExist(product.Product))
                    throw new ShopException($"Product {product.Product.Name} is not registered");

                shop.AddProduct(product.Product, product.Amount);
            }

            return shop.ProductsList;
        }

        public Shop FindShopWithCheapestProducts(List<ProductOrder> products)
        {
            if (products is null) throw new ShopException("Products are null", new ArgumentException());

            return _shops
                .ToDictionary(shop => shop, shop => shop.FindShopProducts(products)?
                    .Sum(shopProduct => shopProduct.Key.Price * shopProduct.Value))
                .OrderBy(keyValuePair => keyValuePair.Value)
                .FirstOrDefault(keyValuePair => keyValuePair.Value != 0).Key;
        }

        public void RegisterProduct(Product product) => RegisterProducts(new List<Product> { product });

        private bool ProductExist(Product product) => _products.Any(p => p.Id == product.Id);
        private bool ShopExist(Shop shop) => _shops.Any(s => s.Id == shop.Id);
    }
}