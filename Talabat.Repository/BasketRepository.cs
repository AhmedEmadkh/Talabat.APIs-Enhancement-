using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var JsonBasket = await _database.StringGetAsync(BasketId);
            return JsonBasket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(JsonBasket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonBasket = JsonSerializer.Serialize(Basket);

            var CreatedOrUpdated = await _database.StringSetAsync(Basket.Id, JsonBasket, TimeSpan.FromDays(1));

            return CreatedOrUpdated ? await GetBasketAsync(Basket.Id) : null;
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }
    }
}
