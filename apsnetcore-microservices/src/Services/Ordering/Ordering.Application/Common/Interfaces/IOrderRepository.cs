using Ordering.Domain.Entities;
using Contracts.Common.Interfaces;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order, long>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);

        void CreateOrder(Order order);

        void DeleteOrder(Order order);

        Task<Order> UpdateOrderAsync(Order order);
    }
}
