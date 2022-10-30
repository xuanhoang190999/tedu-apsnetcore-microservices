using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public CreateOrderCommandHandler(ILogger logger, IMapper mapper, IOrderRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        private const string MethodName = "CreateOrderCommandHandler";

        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - UserName: {request.UserName}");

            var orderEntities = _mapper.Map<Order>(request);

            await _repository.CreateAsync(orderEntities);
            await _repository.SaveChangesAsync();

            _logger.Information($"END: {MethodName} - UserName: {request.UserName}");

            return new ApiSuccessResult<long>(orderEntities.Id);
        }
    }
}
