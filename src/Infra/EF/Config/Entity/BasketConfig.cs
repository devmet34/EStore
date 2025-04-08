using EStore.Core.Entities.BasketAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infra.EF.Config.Entity;
public class BasketConfig : IEntityTypeConfiguration<Basket>
{
  public void Configure(EntityTypeBuilder<Basket> builder)
  {
    builder.Property(b => b.TotalPrice)
      .HasColumnType("decimal(18, 2)");

    builder.HasMany(b => b.BasketItems)
      .WithOne()
      .IsRequired();
  }


}
