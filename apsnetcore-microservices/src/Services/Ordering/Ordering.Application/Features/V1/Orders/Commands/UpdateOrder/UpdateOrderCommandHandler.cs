using AutoMapper;
using MediatR;
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
            _logger.Information($"BEGIN: {MethodName} - UserName: {request.UserName}");

            var order = await _repository.GetByIdAasync(request.Id);
            if (order == null)
                return new ApiErrorResult<OrderDto>("Order not exist.");

            order.UserName = request.UserName;
            order.TotalPrice = request.TotalPrice;
            order.FirstName = request.FirstName;
            order.LastName = request.LastName;
            order.EmailAddress = request.EmailAddress;
            order.ShippingAddress = request.ShippingAddress;
            order.InvoiceAddress = request.InvoiceAddress;

            await _repository.UpdateAsync(order);
            await _repository.SaveChangesAsync();

            var orderDto = _mapper.Map<OrderDto>(order);

            _logger.Information($"END: {MethodName} - UserName: {request.UserName}");

            return new ApiSuccessResult<OrderDto>(orderDto);
        }
    }
}
