using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketsController(IBasketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Entities.Cart), (int)HttpStatusCode.OK)] // Swagger cho biết kiểu trả về
        public async Task<ActionResult<Entities.Cart>> GetBasketByusername([Required] string username)
        {
            var result = await _repository.GetBaskByUserName(username);
            return Ok(result ?? new Entities.Cart());
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Entities.Cart), (int)HttpStatusCode.OK)] // Swagger cho biết kiểu trả về
        public async Task<ActionResult<Entities.Cart>> UpdateBasket([FromBody] Entities.Cart cart)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1)) // Thời gian tồn tại trong 1 giờ.
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Sau 5 phút nếu không có hoạt động sẽ bắt đầu tính thời gian 1 tiếng ở trên để xóa xóa cache.

            var result = await _repository.UpdateBasket(cart, options);
            return Ok(result);
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)] // Swagger cho biết kiểu trả về
        public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
        {
            var result = await _repository.DeteleBasketFromUserName(username);
            return Ok(result);
        }
    }
}
