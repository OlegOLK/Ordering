using Cortex.Mediator.Commands;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Commands;

/// <summary>
/// GetOrderCommand
/// </summary>
/// <seealso cref="Cortex.Mediator.Commands.ICommand&lt;Ordering.Domain.Models.Order&gt;" />
[DataContract]
public class GetOrderCommand : ICommand<Order>
{
    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>
    /// The order identifier.
    /// </value>
    [DataMember]
    public int OrderId { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrderCommand"/> class.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    public GetOrderCommand(int orderId)
    {
        OrderId = orderId;
    }
}
