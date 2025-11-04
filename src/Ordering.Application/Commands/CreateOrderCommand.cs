using Cortex.Mediator.Commands;
using Ordering.Domain.Models;
using System.Runtime.Serialization;

namespace Ordering.Application.Commands;

/// <summary>
/// CreateOrderCommand
/// </summary>
/// <seealso cref="ICommand{bool}" />
[DataContract]
public class CreateOrderCommand : ICommand<int>
{
    /// <summary>
    /// Gets the customer identifier.
    /// </summary>
    /// <value>
    /// The customer identifier.
    /// </value>
    [DataMember]
    public string CustomerId { get; private set; }

    /// <summary>
    /// Gets or sets the order items.
    /// </summary>
    /// <value>
    /// The order items.
    /// </value>
    [DataMember]
    public IEnumerable<OrderItem> OrderItems { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderCommand" /> class.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="orderItems">The order items.</param>
    public CreateOrderCommand(string customerId, IEnumerable<OrderItem> orderItems)
    {
        CustomerId = customerId;
        OrderItems = orderItems;
    }
}
