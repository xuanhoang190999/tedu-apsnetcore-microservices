using MediatR;
using Ordering.Application.Common.Models;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders
{
    public class UpdateOrderCommand : IRequest<ApiResult<OrderDto>>
    {
        public long Id { get; private set; }

        public string UserName { get; set; }

        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }

        public void SetId(long id)
        {
            Id = id;
        }
    }
}
