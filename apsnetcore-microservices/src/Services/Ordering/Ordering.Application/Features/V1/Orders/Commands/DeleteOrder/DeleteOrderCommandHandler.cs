using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public DeleteOrderCommandHandler(ILogger logger, IMapper mapper, IOrderRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        private const string MethodName = "DeleteOrderCommandHandler";

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Id: {request.Id}");

            var orderEntities = await _repository.GetByIdAasync(request.Id);
            if (orderEntities == null)
                throw new NotFoundException(nameof(orderEntities));

            await _repository.DeleteAsync(orderEntities);
            await _repository.SaveChangesAsync();

            _logger.Information($"END: {MethodName} - Id: {request.Id}");

            return Unit.Value;
        }
    }
}
