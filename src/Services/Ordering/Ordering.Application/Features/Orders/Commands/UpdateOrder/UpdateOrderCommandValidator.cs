using FluentValidation;


namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {  // same as for CheckoutOrderCommandValidator
            RuleFor(command => command.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must no exceed 50 characters.");

            RuleFor(command => command.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(command => command.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
