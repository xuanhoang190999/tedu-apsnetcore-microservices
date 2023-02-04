using AutoMapper;
using Contracts.Services;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using Shared.Services.Email;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ISmtpEmailService _emailService;

        public CreateOrderCommandHandler(ILogger logger, IMapper mapper, IOrderRepository repository, ISmtpEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _emailService = emailService;
        }

        private const string MethodName = "CreateOrderCommandHandler";

        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - UserName: {request.UserName}");

            var orderEntity = _mapper.Map<Order>(request);

            _repository.CreateOrder(orderEntity);
            orderEntity.AddedOrder();
            await _repository.SaveChangesAsync();

            _logger.Information($"Order {orderEntity.Id} is successfully created");

            _logger.Information($"END: {MethodName} - UserName: {request.UserName}");
            return new ApiSuccessResult<long>(orderEntity.Id);
        }

        private async Task SendEmailAsync(Order order, CancellationToken cancellationToken)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = order.EmailAddress,
                Body = "Order was created",
                Subject = "Order wax created"
            };

            try
            {
                await _emailService.SendEmailAsync(emailRequest, cancellationToken);
                _logger.Information($"Sent Created Order to email {order.EmailAddress}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Order {order.Id} failed due to an error with the email service: {ex.Message}");
            }
        }
    }
}
