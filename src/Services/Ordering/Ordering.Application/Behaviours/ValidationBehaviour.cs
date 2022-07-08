using FluentValidation;
using MediatR;
using Ordering.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Ordering.Application.Behaviours
{  // MediatR pipeline behaviours are created to run before/after request handlers (basically middlewares)
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {  // class to run the command/query request validators and check for errors
        private readonly IEnumerable<IValidator<TRequest>> _validators;


        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);  // get request as validation context

                var validationResults = await Task.WhenAll(_validators.Select(
                    validator => validator.ValidateAsync(context, cancellationToken)
                ));  // run all request validators

                var validationErrors = validationResults  // get validation errors
                    .SelectMany(result => result.Errors)
                    .Where(failure => failure != null)
                    .ToList();

                if (validationErrors.Count != 0)
                {  // throw custom exception in case of errors
                    throw new CustomValidationException(validationErrors);
                }
            }
            return await next();
        }
    }
}
