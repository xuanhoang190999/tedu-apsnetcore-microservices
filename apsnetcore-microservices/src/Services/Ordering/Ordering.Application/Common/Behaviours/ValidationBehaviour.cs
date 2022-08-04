﻿using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Common.Exceptions.ValidationException;

namespace Ordering.Application.Common.Behaviours
{
    // Quét xem có vấn đề gì với Validation hay không?
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            // Nếu có 1 Validation có lỗi thì sẽ đẩy qua ValidationException.
            if (failures.Any())
                throw new ValidationException(failures);
            return await next();
        }
    }
}