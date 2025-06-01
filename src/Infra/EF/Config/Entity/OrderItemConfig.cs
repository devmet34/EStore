using EStore.Core.Entities;
using EStore.Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infra.EF.Config.Entity;
public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
  public void Configure(EntityTypeBuilder<OrderItem> builder)
  {
    builder.Property<string>(o => o.ProductName)
      .HasColumnType("nvarchar(90)")
      .IsRequired();
    builder.Property(o => o.Price)
      .HasColumnType("decimal(18, 2)");

    builder.HasOne<Product>()
      .WithMany()
      .HasForeignKey(o => o.ProductId)
      .OnDelete(DeleteBehavior.ClientSetNull);

  }


}
