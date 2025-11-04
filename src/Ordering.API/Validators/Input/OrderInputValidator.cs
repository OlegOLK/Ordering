using FluentValidation;
using Ordering.API.Models.Input;

namespace Ordering.API.Validators.Input;

/// <summary>
/// OrderInputValidator
/// </summary>
/// <seealso cref="FluentValidation.AbstractValidator&lt;Ordering.API.Models.Input.OrderInput&gt;" />
public class OrderInputValidator : AbstractValidator<OrderInput> 
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderInputValidator"/> class.
    /// </summary>
    public OrderInputValidator()
    {
        RuleFor(order => order.CustomerId).NotEmpty();
        RuleFor(order => order.Items).NotEmpty();
        RuleForEach(order => order.Items).SetValidator(new OrderItemInputValidator());
    }
}
