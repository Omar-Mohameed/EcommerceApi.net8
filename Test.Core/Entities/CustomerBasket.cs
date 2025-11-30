using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket() { }
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }  // typically the user's ID (key in redis)
        public List<BasketItem> basketItems { get; set; } = new List<BasketItem>(); // list of items in the basket (value in redis)
    }
}
