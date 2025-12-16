using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities.Order;

namespace Test.Core.DTOS.Orders
{
    public record OrderDTO
    {
        public int deliveryMethodId { get; set; }

        public string basketId { get; set; }
        public ShipAddressDTO shipAddress { get; set; }
    }
}
