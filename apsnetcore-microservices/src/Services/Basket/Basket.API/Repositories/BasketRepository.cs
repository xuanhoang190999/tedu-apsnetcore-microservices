using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

using ILogger = Serilog.ILogger; // Đè lên logger của Microsoft

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;

        public BasketRepository(IDistributedCache redisCacheService, ISerializeService serializeService, ILogger logger)
        {
            _redisCacheService = redisCacheService;
            _serializeService = serializeService;
            _logger = logger;
        }

        public async Task<Cart?> GetBaskByUserName(string username)
        {
            _logger.Information($"BEGIN: GetBaskByUserName {username}");
            var basket = await _redisCacheService.GetStringAsync(username);
            _logger.Information($"END: GetBaskByUserName {username}");

            return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            _logger.Information($"BEGIN: UpdateBasket for {cart.Username}");

            if (options != null)
            {
                await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart), options);
            }
            else
            {
                await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
            }

            _logger.Information($"END: UpdateBasket for {cart.Username}");

            return await GetBaskByUserName(cart.Username);
        }

        public async Task<bool> DeteleBasketFromUserName(string userName)
        {
            try
            {
                _logger.Information($"BEGIN: DeteleBasketFromUserName {userName}");
                await _redisCacheService.RemoveAsync(userName);
                _logger.Information($"END: DeteleBasketFromUserName {userName}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("DeteleBasketFromUserName: " + ex.Message);
                throw;
            }
        }
    }
}
