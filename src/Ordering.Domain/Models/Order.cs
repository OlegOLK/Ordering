namespace Ordering.Domain.Models;

/// <summary>
/// Order
/// </summary>
/// <seealso cref="Entity" />
public class Order : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    protected Order()
    {
        _items = new List<OrderItem>();
        Status = OrderStatus.Created;
        CreatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    public Order(string customerId) : this()
    {
        CustomerId = customerId;
    }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>
    /// The customer identifier.
    /// </value>
    public string CustomerId { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the items.
    /// </summary>
    /// <value>
    /// The items.
    /// </value>
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>
    /// The status.
    /// </value>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Gets the created on.
    /// </summary>
    /// <value>
    /// The created on.
    /// </value>
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// The items
    /// </summary>
    private readonly List<OrderItem> _items;

    /// <summary>
    /// Gets the total.
    /// </summary>
    /// <returns></returns>
    public decimal GetTotal() => _items.Sum(x => x.Quantity * x.Price);

    /// <summary>
    /// Adds the order item.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="price">The price.</param>
    /// <param name="quantity">The quantity.</param>
    public void AddOrderItem(int productId, string name, decimal price, int quantity)
    {
        OrderItem orderItem = new(productId, name, price, quantity);

        _items.Add(orderItem);
    }
}
