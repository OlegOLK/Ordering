using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Persistance.Postgres.EntityConfigurations;

/// <summary>
/// OrderItemEntityTypeConfiguration
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;OrderProcessing.Aggregates.Orders.Models.OrderItem&gt;" />
public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder
            .Property(x=> x.Id)
            .UseIdentityColumn();

        builder
            .Property<int>("OrderId");
    }
}
