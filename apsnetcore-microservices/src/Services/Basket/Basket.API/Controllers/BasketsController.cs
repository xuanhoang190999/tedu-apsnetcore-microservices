using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
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
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly IMapper _mapper;
        private readonly StockItemGrpcService _stockItemGrpcService;

        public BasketsController(IBasketRepository repository, IPublishEndpoint publishEndPoint, IMapper mapper, StockItemGrpcService stockItemGrpcService)
        {
            _repository = repository;
            _publishEndPoint = publishEndPoint;
            _mapper = mapper;
            _stockItemGrpcService = stockItemGrpcService;
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
            // Communicate with Inventory.Grpc and check quantity available of products
            foreach(var item in cart.Items)
            {
                var stock = await _stockItemGrpcService.GetStock(item.ItemNo);
                item.SetAvailableQuantity(stock.Quantity);
            }

            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(10)) // Thời gian tồn tại trong 10 giờ.
                .SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Sau 10 phút nếu không có hoạt động sẽ bắt đầu tính thời gian 1 tiếng ở trên để xóa xóa cache.

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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)] // Có 2 kiểu trả về Accepted và NotFound.
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBaskByUserName(basketCheckout.UserName);
            if(basket == null) return NotFound();

            // publish checkout event to EventBus Message
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice; // Tính lại tiền, tránh việc client thay đổi giá ở API

            await _publishEndPoint.Publish(eventMessage);

            // remove the basket
            await _repository.DeteleBasketFromUserName(basketCheckout.UserName);

            return Accepted();
        }
    }
}
