using Ordering.Domain.Models;

namespace Ordering.API.Models.Input;

/// <summary>
/// OrderItemInput
/// </summary>
public class OrderItemInput
{
    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>
    /// The product identifier.
    /// </value>
    public int ProductId { get; set; }
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>
    /// The price.
    /// </value>
    public decimal Price { get; set; }
    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>
    /// The quantity.
    /// </value>
    public int Quantity { get; set; }

    /// <summary>
    /// Converts to domainmodel.
    /// </summary>
    /// <returns></returns>
    public OrderItem ToDomainModel()
    {
        return new OrderItem(ProductId, Name, Price, Quantity);
    }
}
