// GC
// Shopping Car application

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Cart
{

    // interface for items which can be placed in a shopping cart
    public interface ICartAble
    {
        String Code { get; }                    // unique in a cart
        String Description { get; }
        double Price { get; }                   // in euros
    }

    // an item in a cart
    public class CartItem : ICartAble
    {
        private String code;        
        
        public String Code
        {
            get
            {
                return code;
            }
            set
            {
                // validate code - min 5 chars, no white space
                if (String.IsNullOrWhiteSpace(value) || (value.Length < 5))
                {
                    throw new ArgumentException("code invalid");
                }
                else
                {
                    this.code = value;
                }
            }
        }
        public String Description { get; set; }
        public double Price { get; set; }                       // euro

        // additional property:
        public int Quantity { get; set; }                       // >= 1

        // to do: validation...

        public override String ToString()
        {
            return "Code:" + Code + "  description: " + Description + "  price: " + Price + " quantity: " + Quantity;
        }

    }

    // an electrical item can be an item in a shopping cart
    public class ElectricalItem : ICartAble
    {
        public String Code { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }

        // additional property
        public bool Weee { get; set; }              // recyclable under WEEE directive
    }

    public enum DeliveryMethod
    {
        Priority, Standard          // more delivery options possible
    }

    // a shopping cart
    public class ShoppingCart
    {
        // shipping costs
        public const double CostPerItemPriority = 2.00;                            // per item for priority delivery
        public const double CostPerItemStandard = 0.80;                            // per item for standard delivery

        private List<CartItem> items;                                               // collection of items 

        public ShoppingCart()
        {
            items = new List<CartItem>();
        }

        // read only property to get items collection
        public Collection<CartItem> Items
        {
            get
            {
                return new Collection<CartItem>(items);
            }
        }

        // indexer - pull out matching item for specified code
        public CartItem this[String code]
        {
            get
            {
                CartItem item = items.FirstOrDefault(i => i.Code.ToUpper(CultureInfo.CurrentCulture) == code.ToUpper(CultureInfo.CurrentCulture));
                if (item != null)
                {
                    return item;
                }
                else
                {
                    throw new ArgumentException("no item with matching product code found in cart");
                }
            }
        }

        // add an item to the cart i.e. increase qty of that item in the cart
        // +
        public void AddItem(ICartAble item)
        {
            // code is unique
            var match = items.FirstOrDefault(i => i.Code.ToUpper(CultureInfo.CurrentCulture) == item.Code.ToUpper(CultureInfo.CurrentCulture));
            if (match == null)
            {
                items.Add(new CartItem() { Code = item.Code, Description = item.Description, Price = item.Price, Quantity = 1 });
            }
            else
            {
                match.Quantity++;                   // increase
            }
        }


        // remove item from cart i.e. decrease qty of that item in the cart
        // -
        public void RemoveItem(ICartAble item)
        {
            var match = items.FirstOrDefault(i => i.Code.ToUpper(CultureInfo.CurrentCulture) == item.Code.ToUpper(CultureInfo.CurrentCulture));
            if (match != null)
            {
                if (match.Quantity == 1)
                {
                    items.Remove(match);            // not in cart any more
                }
                else
                {
                    match.Quantity--;               // reduce quantity
                }
            }
            else
            {
                throw new ArgumentException("item not in cart");
            }

        }

        // calculate total price of cart
        public double CalculateTotalPrice()
        {
            return items.Sum(item => item.Price * item.Quantity);
        }

        // calculate the price of shipping
        public double CalculateShippingCost(DeliveryMethod shipping)
        {
            double shippingCost;

            if (shipping == DeliveryMethod.Priority)
            {
                shippingCost = items.Sum(i => i.Quantity) * CostPerItemPriority;
            }
            else                // DeliveryMethod.Standard
            {
                shippingCost = items.Sum(i => i.Quantity) * CostPerItemStandard;
            }
           
            return shippingCost;
        }

   }
}
