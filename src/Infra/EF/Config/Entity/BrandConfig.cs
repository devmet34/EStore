using EStore.Core.Entities;
using EStore.Core.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Config.Entity;
public class BrandConfig : IEntityTypeConfiguration<Brand>
{
  public void Configure(EntityTypeBuilder<Brand> builder)
  {
    //builder.OwnsOne(b => b.Address);
    //builder.Navigation(b => b.BAddress).IsRequired();
  }
}
