using Contracts.Common.Interfaces;
using Contracts.Domains;

namespace Contracts.Common.Events
{
    public class EventEntity<T> : EntityBase<T>, IEventEntity<T> // Dùng cho tất cả các Service nào muốn sử dụng, khiển trai DDD
    {
        private readonly List<BaseEvent> _domainEvents = new();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public IReadOnlyCollection<BaseEvent> DomainEvents() => _domainEvents.AsReadOnly(); // Trả ra dạng read only => Chỉ trả ra và không cho sửa list đã trả ra rồi

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }
}
