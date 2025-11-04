using Ordering.Domain.Models;

namespace Ordering.API.Models.Input;

/// <summary>
/// OrderInput
/// </summary>
public class OrderInput
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>
    /// The customer identifier.
    /// </value>
    public string CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the items.
    /// </summary>
    /// <value>
    /// The items.
    /// </value>
    public IEnumerable<OrderItemInput> Items { get; set; }

    /// <summary>
    /// Converts to domainmodel.
    /// </summary>
    /// <returns></returns>
    public Order ToDomainModel()
    {
        Order order = new Order(CustomerId);
        foreach (OrderItemInput item in Items)
        {
            order.AddOrderItem(item.ProductId, item.Name, item.Price, item.Quantity);
        }

        return order;
    }
}
