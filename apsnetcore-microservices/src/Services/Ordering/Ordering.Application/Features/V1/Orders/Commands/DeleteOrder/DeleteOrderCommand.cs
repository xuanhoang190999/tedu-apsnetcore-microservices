﻿using MediatR;

namespace Ordering.Application.Features.V1.Orders
{
    public class DeleteOrderCommand : IRequest
    {
        public long Id { get; set; }

        public DeleteOrderCommand(long id)
        {
            Id = id;
        }
    }
}
