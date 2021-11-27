using System.Collections.Generic;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Services
{
    public interface IShopManager
    {
        Shop CreateShop(Shop shop);
        List<Product> AddProductsToShop(Shop shop, List<ProductOrder> products);
        Shop FindShopWithCheapestProducts(List<ProductOrder> products);
        void RegisterProducts(List<Product> products);
    }
}