using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Cart?> GetBaskByUserName(string userName);
        Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null); // DistributedCacheEntryOptions Params thông báo tồn tại trong bao lâu
        Task<bool> DeteleBasketFromUserName(string userName);
    }
}
