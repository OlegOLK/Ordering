using Ordering.Domain.Exceptions;

namespace Ordering.Domain.Models;

/// <summary>
/// OrderItem
/// </summary>
/// <seealso cref="Entity" />
public class OrderItem : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItem" /> class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="price">The price.</param>
    /// <param name="quantity">The quantity.</param>
    /// <exception cref="Orders.Exceptions.OrderException">Invalid quantity</exception>
    public OrderItem(int productId, string name, decimal price, int quantity)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        Quantity = quantity <= 0 ? throw new OrderException("Invalid quantity") : quantity;
    }

    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>
    /// The product identifier.
    /// </value>
    public int ProductId { get; private set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    /// Ideally should be fetched from Items DB
    public string Name { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>
    /// The price.
    /// </value>
    /// Ideally should be fetched from Items DB
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>
    /// The quantity.
    /// </value>
    public int Quantity { get; private set; }
}
