using System.Collections.Generic;
using NUnit.Framework;
using Shops.Entities;
using Shops.Services;

namespace Shops.Tests
{
    [TestFixture]
    public class Tests
    {
        private static ShopManager _shopManager;
        private static Customer _customer;
        private static List<Product> _products1;
        private static List<Product> _products2;
        private static List<Product> _products3;
        private static List<ProductOrder> _productsToDeliver1;
        private static List<ProductOrder> _productsToDeliver2;
        private static List<ProductOrder> _productsToDeliver3;

        [SetUp]
        public void Setup() => _shopManager = new ShopManager();

        [TestCase(275000, 100000, 1)]
        [TestCase(200000, 50000, 4)]
        [TestCase(300000, 3000, 100)]
        public void AddProductsToShopAndBuy_CustomerMoneyDecreased(int moneyBefore, int productPrice, int productToBuyCount)
        {
            var product = new Product("MacBook Air 2020", productPrice);
            _products1 = new List<Product> {product};
            Shop reStore = _shopManager.CreateShop(new Shop("re:store", new Address("Pushkin's street", 1), 10000000));

            _shopManager.RegisterProduct(product);

            _productsToDeliver1 = new List<ProductOrder> {new(product, productToBuyCount)};
            _shopManager.AddProductsToShop(reStore, _productsToDeliver1);

            _customer = new Customer("Misha Libchenko", moneyBefore);

            for (int i = 0; i < productToBuyCount; ++i)
                reStore.BuyProduct(_customer, product);

            int boughtProductPrice = productPrice * productToBuyCount;

            Assert.AreEqual(moneyBefore - boughtProductPrice, _customer.Money);
        }

        [TestCase(275000, 100000, 1)]
        public void AddProductsToShopAndChangePrice_PriceChanged(int priceBefore, int priceAfter, int productToBuyCount)
        {
            Shop reStore = _shopManager.CreateShop(new Shop("re:store", new Address("Pushkin's street", 1), 10000000));
            var product = new Product("IPhone XR", priceBefore);

            _shopManager.RegisterProduct(product);

            _productsToDeliver1 = new List<ProductOrder> {new(product, productToBuyCount)};
            _shopManager.AddProductsToShop(reStore, _productsToDeliver1);
            reStore.ChangeProductPrice(product, priceAfter);

            Assert.AreEqual(priceAfter, reStore.GetProductPrice(product));
        }

        [Test]
        public void FindShopWithCheapestProducts_ShopFound()
        {
            Shop mVideo = _shopManager.CreateShop(new Shop("MVideo", new Address("Pushkin street", 2), 5000000));
            Shop reStore = _shopManager.CreateShop(new Shop("re:store", new Address("Pushkin street", 1), 10000000));
            Shop eldorado = _shopManager.CreateShop(new Shop("Eldorado", new Address("Balkanskaya street", 12), 10000000));
            Shop lenta = _shopManager.CreateShop(new Shop("Lenta", new Address("Balkanskaya street", 15), 8000000));
            Shop iPort = _shopManager.CreateShop(new Shop("iPort", new Address("Sennaya square", 5), 9999999));

            Product milkshake = new ("Milkshake", 200);
            Product iPhone = new ("Iphone XR", 65000);
            Product macBookAir = new ("MacBook Air 2020 M1", 100000);
            Product macBookPro = new ("MacBook Pro 2020 i7", 125000);
            Product airPodsPro = new ("AirPods Pro", 15000);

            var techShops = new List<Shop> { reStore, mVideo, eldorado, iPort };
            var appleShops = new List<Shop> { reStore, iPort };
            var supermarkets = new List<Shop> { lenta };

            _products1 = new List<Product> { iPhone, macBookAir };
            _products2 = new List<Product> { macBookPro, airPodsPro };
            _products3 = new List<Product> { milkshake };

            _shopManager.RegisterProducts(_products1);
            _shopManager.RegisterProducts(_products2);
            _shopManager.RegisterProducts(_products3);

            _productsToDeliver1 = new List<ProductOrder> { new (iPhone, 10), new (macBookAir, 5) };
            _productsToDeliver2 = new List<ProductOrder> { new (macBookPro, 10), new (airPodsPro, 15), new (macBookAir, 20) };
            _productsToDeliver3 = new List<ProductOrder> { new (milkshake, 30) };

            foreach (Shop shop in techShops) _shopManager.AddProductsToShop(shop, _productsToDeliver1);
            foreach (Shop shop in appleShops) _shopManager.AddProductsToShop(shop, _productsToDeliver2);
            foreach (Shop shop in supermarkets) _shopManager.AddProductsToShop(shop, _productsToDeliver3);

            reStore.ChangeProductPrice(iPhone, 50000);
            reStore.ChangeProductPrice(macBookAir, 110000);
            reStore.ChangeProductPrice(macBookPro, 100000);
            reStore.ChangeProductPrice(airPodsPro, 12500);

            iPort.ChangeProductPrice(iPhone, 70000);
            iPort.ChangeProductPrice(macBookPro, 90000);
            iPort.ChangeProductPrice(macBookAir, 80000);
            iPort.ChangeProductPrice(airPodsPro, 20000);

            mVideo.ChangeProductPrice(macBookAir, 75000);
            mVideo.ChangeProductPrice(iPhone, 50000);

            eldorado.ChangeProductPrice(iPhone, 45000);
            eldorado.ChangeProductPrice(macBookAir, 75000);

            lenta.ChangeProductPrice(milkshake, 50);

            var productsToBuy = new List<ProductOrder>
            {
                new (iPhone, 8), new (macBookAir, 3),
                new (airPodsPro, 8), new (macBookPro, 6),
            };

            Assert.AreEqual(reStore, _shopManager.FindShopWithCheapestProducts(productsToBuy));
        }

        [Test]
        public void FindShopWithCheapestProducts_ShopNotFound()
        {
            Shop mVideo = _shopManager.CreateShop(new Shop("MVideo", new Address("Pushkin street", 2), 5000000));
            Shop reStore = _shopManager.CreateShop(new Shop("re:store", new Address("Pushkin street", 1), 10000000));

            Product iPhone = new ("Iphone XR", 65000);
            Product macBookAir = new ("MacBook Air 2020 M1", 100000);

            _products1 = new List<Product> { iPhone };
            _products2 = new List<Product> { macBookAir };

            _shopManager.RegisterProducts(_products1);
            _shopManager.RegisterProducts(_products2);

            _productsToDeliver1 = new List<ProductOrder> { new (iPhone, 10) };
            _productsToDeliver2 = new List<ProductOrder> { new (macBookAir, 5) };

            _shopManager.AddProductsToShop(mVideo, _productsToDeliver1);
            _shopManager.AddProductsToShop(reStore, _productsToDeliver2);

            var productsToBuy = new List<ProductOrder> { new (iPhone, 15), new (macBookAir, 10) };

            Assert.Null(_shopManager.FindShopWithCheapestProducts(productsToBuy));
        }

        [TestCase(5000000, 65000, 100000, 2, 1, 1)]
        public void BuyСonsignmentOfProducts_MoneyIncreasedAndProductAmountChanged(int moneyBefore, int iPhonePrice,
            int macBookPrice, int iPhoneToBuy, int macBookToBuy, int basePrice)
        {
            Shop mVideo = _shopManager.CreateShop(new Shop("MVideo", new Address("Pushkin street", 2), moneyBefore));
            _customer = new Customer("Misha Libchenko", 300000);
            
            Product iPhone = new ("Iphone XR", basePrice);
            Product macBookAir = new ("MacBook Air 2020 M1", basePrice);

            _products1 = new List<Product> { iPhone, macBookAir };

            _shopManager.RegisterProducts(_products1);

            _productsToDeliver1 = new List<ProductOrder> { new (iPhone, iPhoneToBuy), new (macBookAir, macBookToBuy) };

            _shopManager.AddProductsToShop(mVideo, _productsToDeliver1);

            mVideo.ChangeProductPrice(iPhone, iPhonePrice);
            mVideo.ChangeProductPrice(macBookAir, macBookPrice);

            mVideo.BuyProducts(_customer, _productsToDeliver1);

            int moneyAfterSellProducts = moneyBefore + iPhonePrice * iPhoneToBuy + macBookPrice * macBookToBuy -
                                         basePrice * (macBookToBuy + iPhoneToBuy);

            Assert.AreEqual(moneyAfterSellProducts, mVideo.Money);
            Assert.AreEqual(mVideo.GetProductAmount(iPhone), 0);
            Assert.AreEqual(mVideo.GetProductAmount(macBookAir), 0);
        }
    }
}