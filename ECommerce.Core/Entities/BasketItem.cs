using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class BasketItem
    {
        public int Id { get; set; } // product id
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int Qunatity { get; set; }
        public decimal Price { get; set; }
    }
}
