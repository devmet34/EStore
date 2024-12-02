using Estore.Core.Entities;
using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF
{
    public class EStoreDbContext : DbContext
  {
    #pragma warning disable CS8618 // Required by Entity Framework    
    public EStoreDbContext(DbContextOptions<EStoreDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public DbSet<Order> Orders { get; set; }

   
  


    protected override void OnModelCreating(ModelBuilder builder)
    {
      //builder.HasSequence("seq_prod_id").StartsAt(1000).IncrementsBy(1);
      base.OnModelCreating(builder);
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //mc get entity db configs from classes with IEntityTypeConfiguration. 
      //builder.Entity<BasketItem>().Property(b => b.ProductName).HasColumnType("nvarchar(90)");
        
    }
  }//eo class
}
