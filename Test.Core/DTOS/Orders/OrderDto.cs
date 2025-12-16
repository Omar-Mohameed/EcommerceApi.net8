using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.Entities.Order;

namespace Ecom.Core.DTOS.Orders
{
    public record OrderDto
    {
        public int deliveryMethodId { get; set; }

        public string basketId { get; set; }
        public ShipAddressDTO shipAddress { get; set; }
    }
}
