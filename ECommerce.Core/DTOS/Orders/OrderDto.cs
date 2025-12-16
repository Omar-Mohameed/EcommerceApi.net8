using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTOS.Orders
{
    public record OrderDTO
    {
        public int deliveryMethodId { get; set; }

        public string basketId { get; set; }
        public ShipAddressDTO shipAddress { get; set; }
    }
}
