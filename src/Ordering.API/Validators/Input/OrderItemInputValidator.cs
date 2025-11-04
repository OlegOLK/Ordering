using FluentValidation;
using Ordering.API.Models.Input;

namespace Ordering.API.Validators.Input;

/// <summary>
/// OrderItemInputValidator
/// </summary>
/// <seealso cref="FluentValidation.AbstractValidator&lt;Ordering.API.Models.Input.OrderItemInput&gt;" />
public class OrderItemInputValidator : AbstractValidator<OrderItemInput>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItemInputValidator"/> class.
    /// </summary>
    public OrderItemInputValidator()
    {
        RuleFor(orderItem => orderItem.Quantity).GreaterThan(0);
        RuleFor(orderItem => orderItem.ProductId).GreaterThan(-1);
    }
}
