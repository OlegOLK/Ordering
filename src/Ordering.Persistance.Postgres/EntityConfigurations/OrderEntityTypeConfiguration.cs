using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Persistance.Postgres.EntityConfigurations;

/// <summary>
/// OrderEntityTypeConfiguration
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;OrderProcessing.Aggregates.Orders.Models.Order&gt;" />
public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder
            .Property(x=> x.CustomerId)
            .IsRequired();

        builder
            .Property(x=> x.Status)
            .HasConversion<string>()
            .HasMaxLength(30);
    }
}
