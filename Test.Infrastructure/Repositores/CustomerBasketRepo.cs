using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Test.Core.Entities;
using Test.Core.Interfaces;

namespace Test.Infrastructure.Repositores
{
    public class CustomerBasketRepo : ICustomerBasketRepo
    {
        private readonly IDatabase _database;
        public CustomerBasketRepo(IConnectionMultiplexer redis) // // IConnectionMultiplexer is responsible for managing the connection between your project and Redis server.
        {
            _database = redis.GetDatabase(); // // get the redis database instance بياخد database من الـ Redis Connection (Shared Singleton).
        }
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.Id,
                JsonSerializer.Serialize(basket), TimeSpan.FromDays(3)); // تخزين قيمة الـ key في الريديز
            if (created)
            {
                return await GetBasketAsync(basket.Id);
            }
            return null;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }
    }
}
