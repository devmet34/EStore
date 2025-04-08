using EStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infra.EF.Config.Entity;
public class BrandConfig : IEntityTypeConfiguration<Brand>
{
  public void Configure(EntityTypeBuilder<Brand> builder)
  {
    //builder.OwnsOne(b => b.Address);
    //builder.Navigation(b => b.BAddress).IsRequired();
  }
}
