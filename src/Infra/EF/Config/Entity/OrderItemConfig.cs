using Estore.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Config.Entity;
public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
  public void Configure(EntityTypeBuilder<OrderItem> builder)
  {
    builder.HasOne(o => o.Product)
      .WithMany()
      .HasForeignKey(o => o.ProductId)
      .OnDelete(DeleteBehavior.ClientSetNull);
      
  }


}
