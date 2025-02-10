using Estore.Core.Entities;
using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Config.Entity;
public class OrderConfig: IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.Property(o => o.TotalPrice)
     .HasColumnType("decimal(18, 2)");

    builder.HasOne<CustomerAddress>()
      .WithMany()
      .HasForeignKey(o => o.CustomerAddressId)
      .OnDelete(DeleteBehavior.SetNull);
      
    
     
  }
}
