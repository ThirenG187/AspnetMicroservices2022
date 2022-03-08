using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task DeleteBasket(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }

    public async Task<ShoppingCart> GetBasket(string username)
    {
        var basketJSON = await _redisCache.GetStringAsync(username);

        if (string.IsNullOrEmpty(basketJSON))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<ShoppingCart>(basketJSON);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        await _redisCache.SetStringAsync(basket.Username, JsonConvert.SerializeObject(basket));
        return await GetBasket(basket.Username);
    }
}
