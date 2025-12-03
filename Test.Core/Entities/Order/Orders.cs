using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Entities.Order
{
    public class Orders : BaseEntity<int>
    {
        public Orders() { }

        public Orders(string buyerEmail, decimal subTotal, List<OrderItem> orderItems, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod)
        {
            BuyerEmail = buyerEmail;
            SubTotal = subTotal;
            OrderItems = orderItems;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
        }

        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; } // total price for product only without adding shapping price 
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderItem> OrderItems { get; set; } = new();
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set;}

        public Status Status { get; set; } = Status.Pending;

        public decimal GetTotal()
        {
            return SubTotal + (DeliveryMethod?.Price ?? 0);
        }
    }
}
