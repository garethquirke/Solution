using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Cart;

namespace ShoppingCarTUnitTestProject
{
    [TestClass]
    public class ShoppingCartUnitTest
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestValidation1()
        {
            CartItem c = new CartItem() { Code = "", Description = "an item", Price = 0, Quantity = 1 };
        }

        [TestMethod]
        public void TestAdd()
        {
            ElectricalItem item1 = new ElectricalItem() { Code = "12345", Description = "LG 42 inch LED tv", Price = 1000, Weee = true };
            ElectricalItem item2 = new ElectricalItem() { Code = "23456", Description = "DeLonghi Bean to Cup Coffee Maker", Price = 600, Weee = true };
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(item1);
            cart.AddItem(item1);        // quantity == 2
            cart.AddItem(item2);

            List<CartItem> items = new List<CartItem> (cart.Items);
            Assert.AreEqual(items.Count, 2);
            Assert.AreEqual(items[0].Quantity, 2);
            Assert.AreEqual(items[1].Quantity, 1);
        }

        [TestMethod]
        public void TestRemove()
        {
            ElectricalItem item1 = new ElectricalItem() { Code = "12345", Description = "LG 42 inch LED tv", Price = 1000, Weee = true };
            ElectricalItem item2 = new ElectricalItem() { Code = "23456", Description = "DeLonghi Bean to Cup Coffee Maker", Price = 600, Weee = true };
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(item1);
            cart.AddItem(item1);
            cart.AddItem(item1);
            cart.AddItem(item2);
            cart.RemoveItem(item2);
            cart.RemoveItem(item1);

            List<CartItem> items = new List<CartItem>(cart.Items);
            Assert.AreEqual(items.Count, 1);
            Assert.AreEqual(items[0].Quantity, 2);
            Assert.AreEqual(items[0].Code, item1.Code);
        }

        [TestMethod]
        public void TestIndexer()
        {
            ElectricalItem item1 = new ElectricalItem() { Code = "12345", Description = "LG 42 inch LED tv", Price = 1000, Weee = true };
            ElectricalItem item2 = new ElectricalItem() { Code = "23456", Description = "DeLonghi Bean to Cup Coffee Maker", Price = 600, Weee = true };
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(item1);
            cart.AddItem(item2);
            var item = cart["23456"];                       // search
            Assert.AreEqual(item.Code, "23456");
            Assert.AreEqual(item.Price, 600);
            Assert.AreEqual(item.Quantity, 1);
        }

        [TestMethod]
        public void TestCalcTotalPrice()
        {
            // test logic to total price for items in cart
            ElectricalItem item1 = new ElectricalItem() { Code = "12345", Description = "LG 42 inch LED tv", Price = 1000, Weee = true };    
            ElectricalItem item2 = new ElectricalItem() { Code = "23456", Description = "DeLonghi Bean to Cup Coffee Maker", Price = 600, Weee = true };
            ElectricalItem item3 = new ElectricalItem() { Code = "34567", Description = "Dyson Ball Vacuum Cleaner", Price = 900, Weee = true };
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(item1);
            cart.AddItem(item2);
            cart.AddItem(item2);
            cart.AddItem(item3);
          
            Assert.AreEqual(cart.CalculateTotalPrice(), item1.Price + item2.Price * 2 + item3.Price);
        }

        [TestMethod]
        public void TestShippingCost()
        {
            ElectricalItem item1 = new ElectricalItem() { Code = "12345", Description = "LG 42 inch LED tv", Price = 10, Weee = true };
            ElectricalItem item2 = new ElectricalItem() { Code = "23456", Description = "DeLonghi Bean to Cup Coffee Maker", Price = 10, Weee = true };
            ElectricalItem item3 = new ElectricalItem() { Code = "34567", Description = "Dyson Ball Vacuum Cleaner", Price = 10, Weee = true };
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(item1);
            cart.AddItem(item2);
            cart.AddItem(item2);        
            cart.AddItem(item2);        // in 3 times
            cart.AddItem(item3);
          
            // 5 items
            Assert.AreEqual(cart.CalculateShippingCost(DeliveryMethod.Priority), 5 * ShoppingCart.CostPerItemPriority);
            Assert.AreEqual(cart.CalculateShippingCost(DeliveryMethod.Standard), 5 * ShoppingCart.CostPerItemStandard);
        }
    }
}
