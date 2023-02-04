using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly ILogger _logger;
        private readonly IOrderRepository _repository;

        public DeleteOrderCommandHandler(ILogger logger, IOrderRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        private const string MethodName = "DeleteOrderCommandHandler";

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Id: {request.Id}");

            var orderEntitiy = await _repository.GetByIdAasync(request.Id);
            if (orderEntitiy == null)
                throw new NotFoundException(nameof(orderEntitiy), request.Id);

            _repository.DeleteOrder(orderEntitiy);
            orderEntitiy.DeleteOrder();
            await _repository.SaveChangesAsync();

            _logger.Information($"Order {orderEntitiy.Id} was successfully deleted");

            return Unit.Value;
        }
    }
}
