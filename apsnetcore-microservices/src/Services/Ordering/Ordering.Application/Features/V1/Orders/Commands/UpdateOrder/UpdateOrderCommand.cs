using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders
{
    public class UpdateOrderCommand  : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>
    {
        public long Id { get; private set; }

        public void SetId(long id)
        {
            Id = id;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.Status, opts => opts.Ignore()) // Tùy nghiệp vụ, nếu muốn update status phải thông qua 1 handle khác
                .IgnoreAllNonExisting();
        }

    }
}
