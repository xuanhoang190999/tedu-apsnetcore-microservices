using MediatR;
using Ordering.Domain.OrderAggregate.Events;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders
{
    public class OrdersDomainHandler :
        INotificationHandler<OrderCreatedEvent>,
        INotificationHandler<OrderDeletedEvent>
    {

        private readonly ILogger _logger;

        public OrdersDomainHandler(
            ILogger logger
        )
        {
            _logger = logger;
        }

        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information("Order Domain Event: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }

        public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information("Order Domain Event: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
