using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EStore.Core.Entities.OrderAggregate;

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
    /*
    builder.HasOne(o => o.Product)
      .WithMany()
      .HasForeignKey(o => o.ProductId)
      .OnDelete(DeleteBehavior.ClientSetNull);
      */
  }


}
