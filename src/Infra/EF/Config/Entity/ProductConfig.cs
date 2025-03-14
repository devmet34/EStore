using EStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Config.Entity
{
  public class ProductConfig : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {

      //builder.ToTable("Products");


      builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(30);

      builder.Property(p => p.Price)
        .IsRequired()
        .HasColumnType("decimal(18,2)");

      builder.Property(p => p.Version)
        .IsRowVersion();

      builder.Property(p => p.SortOrder)
        .IsRequired(false)
        .HasDefaultValue((short)999);

      builder.HasOne(p => p.Brand)
        .WithMany()
        .HasForeignKey(p => p.BrandId);
              

      builder.HasOne(p => p.Category)
        .WithMany()
        .HasForeignKey(p => p.CategoryId);

      builder.Navigation(p => p.Category);
      //todo check loading brand when there is no nav set explicitly
     

        


            

        }
  }
}
