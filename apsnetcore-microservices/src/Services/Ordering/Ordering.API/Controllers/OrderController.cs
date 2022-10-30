using AutoMapper;
using Contracts.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Ordering.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMessageProducer _messageProducer;
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderController(IMediator mediator, IMessageProducer messageProducer, IOrderRepository repository, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Muốn controller này phải có name đầy đủ.
        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string CreateOrder = nameof(CreateOrder);
        }

        [HttpGet("{userName}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByUserName([Required] string userName)
        {
            var query = new GetOrdersQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = RouteNames.CreateOrder)]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<long>> CreateOrder([FromBody] OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var addedOrder = await _repository.CreateOrder(order);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<OrderDto>(addedOrder);
            _messageProducer.SendMessage(result);
            return Ok(result);
        }

        //// Muốn controller này phải có name đầy đủ.
        //private static class RouteNames
        //{
        //    public const string GetOrders = nameof(GetOrders);
        //    public const string CreateOrder = nameof(CreateOrder);
        //    public const string UpdateOrder = nameof(UpdateOrder);
        //    public const string DeleteOrder = nameof(DeleteOrder);
        //}

        //[HttpGet("{userName}", Name = RouteNames.GetOrders)]
        //[ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByUserName([Required] string userName)
        //{
        //    var query = new GetOrdersQuery(userName);
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        //[HttpPost(Name = RouteNames.CreateOrder)]
        //[ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<long>> CreateOrder([FromBody] CreateOrderCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        //[HttpPut("{id:long}", Name = RouteNames.UpdateOrder)]
        //[ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<OrderDto>> UpdateOrder([Required] long id, [FromBody] UpdateOrderCommand command)
        //{
        //    command.SetId(id);
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        //[HttpDelete("{id:long}", Name = RouteNames.DeleteOrder)]
        //[ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        //public async Task<ActionResult> DeleteOrder([Required] long id)
        //{
        //    var command = new DeleteOrderCommand(id);
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}
    }
}
