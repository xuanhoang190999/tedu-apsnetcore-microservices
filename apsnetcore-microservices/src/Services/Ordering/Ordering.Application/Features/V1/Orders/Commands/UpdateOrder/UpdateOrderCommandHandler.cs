using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public UpdateOrderCommandHandler(ILogger logger, IMapper mapper, IOrderRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        private const string MethodName = "UpdateOrderCommandHandler";

        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _repository.GetByIdAasync(request.Id);
            if (orderEntity is null)
                throw new NotFoundException(nameof(Order), request.Id);

            _logger.Information($"BEGIN: {MethodName} - Order: {request.Id}");

            orderEntity = _mapper.Map(request, orderEntity);

            var updateOrder = await _repository.UpdateOrderAsync(orderEntity);
            await _repository.SaveChangesAsync();

            _logger.Information($"Order {request.Id} was successfully updated");

            var orderDto = _mapper.Map<OrderDto>(updateOrder);

            _logger.Information($"END: {MethodName} - Order: {request.Id}");

            return new ApiSuccessResult<OrderDto>(orderDto);
        }
    }
}
