using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOrderRepository _repository;

        public GetOrdersQueryHandler(IMapper mapper, ILogger logger, IOrderRepository repository)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        private const string MethodName = "GetOrdersQueryHandler";

        public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - UserName: {request.UserName}");

            var orderEntities = await _repository.GetOrdersByUserName(request.UserName);
            var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

            _logger.Information($"END: {MethodName} - UserName: {request.UserName}");

            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
    }
}
