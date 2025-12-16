using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities.Order
{
    public class DeliveryMethod: BaseEntity<int>
    {
        public DeliveryMethod(string name, string description, decimal price, string deliveryTime)
        {
            Name = name;
            Description = description;
            Price = price;
            DeliveryTime = deliveryTime;
        }
        public DeliveryMethod()
        {
            
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; }
    }
}
