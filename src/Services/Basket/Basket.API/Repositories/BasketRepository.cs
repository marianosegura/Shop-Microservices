using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public readonly IDistributedCache _redisCache;


        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }


        public async Task<ShoppingCart> GetBasket(string userName)
        {  // get basket as string and return as ShoppingCart object
            string basketJson = await _redisCache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basketJson))
            {
                return null;
            }
            var basket = JsonConvert.DeserializeObject<ShoppingCart>(basketJson);
            return basket;
        }


        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            string basketJson = JsonConvert.SerializeObject(basket);
            await _redisCache.SetStringAsync(basket.UserName, basketJson);
            return await GetBasket(basket.UserName);
        }


        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
