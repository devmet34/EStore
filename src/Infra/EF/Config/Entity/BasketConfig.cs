using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estore.Core.Entities.BasketAggregate;

namespace EStore.Infra.EF.Config.Entity;
public class BasketConfig : IEntityTypeConfiguration<Basket>
{
  public void Configure(EntityTypeBuilder<Basket> builder)
  {
    builder.HasMany(b => b.BasketItems)
      .WithOne()      
      .IsRequired();
  }


}
