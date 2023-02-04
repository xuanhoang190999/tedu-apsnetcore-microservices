using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator, ILogger logger) : base(options)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public DbSet<Order> Orders { get; set; }

        private List<BaseEvent> _baseEvents { get; set; }

        private void SetBaseEventsBeforeSaveChanges()
        {
            var domainEntities = ChangeTracker.Entries<IEventEntity>()
                .Select(x => x.Entity)
                .Where(x => x.DomainEvents().Any())
                .ToList();

            _baseEvents = domainEntities
                .SelectMany(x => x.DomainEvents())
                .ToList();

            domainEntities.ForEach(x => x.ClearDomainEvents()); // Clear các Event Entity cho lần xử lý này.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetBaseEventsBeforeSaveChanges();

            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                    e.State == EntityState.Added ||
                    e.State == EntityState.Deleted);

            foreach (var item in modified)
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addedEntity) // Nếu entity có triển khai IDateTracking.
                        {
                            addedEntity.CreatedDate = DateTime.UtcNow;
                            item.State = EntityState.Added;
                        }
                        break;
                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false; // Nếu Modified có filed Id thì không cho sửa.
                        if (item.Entity is IDateTracking modifiedEntity)
                        {
                            modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                            item.State = EntityState.Modified;
                        }
                        break;
                }
            }


            var result = await base.SaveChangesAsync(cancellationToken);
            await _mediator.DispatchDomainEventAsync(_baseEvents, _logger);

            return result;
        }
    }
}
