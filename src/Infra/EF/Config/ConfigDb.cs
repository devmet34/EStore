using EStore.Infra.EF.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EStore.Infra.EF.Config
{
  public static class ConfigDb
  {
    private static void GetConnectionStrings(IConfiguration config, out string connectionStringApp, out string connectionStringIdentity)
    {
      connectionStringApp = config["Sql:ConnectionStrings:Estore"] ?? throw new InvalidOperationException("Connection string 'EStore' not found.");
      connectionStringIdentity = config["Sql:ConnectionStrings:Identity"] ?? throw new InvalidOperationException("Connection string 'Identity' not found.");

    }

    public static WebApplicationBuilder AddDBContexts(this WebApplicationBuilder builder)
    {
      GetConnectionStrings(builder.Configuration, out string connectionStringApp, out string connectionStringIdentity);

      if (builder.Environment.IsDevelopment())
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
      builder.Services.AddDbContext<EstoreIdentityDbContext>(options => options.UseSqlServer(connectionStringIdentity));
      builder.Services.AddDbContext<EStoreDbContext>(options => options.UseSqlServer(connectionStringApp));
      return builder;

    }

    public static WebApplicationBuilder AddDBContextsPool(this WebApplicationBuilder builder)
    {
      GetConnectionStrings(builder.Configuration, out string connectionStringApp, out string connectionStringIdentity);
      if (builder.Environment.IsDevelopment())
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
      builder.Services.AddDbContext<EstoreIdentityDbContext>(options => options.UseSqlServer(connectionStringIdentity));
      builder.Services.AddDbContextPool<EStoreDbContext>(options => options.UseSqlServer(connectionStringApp));
      return builder;

    }

    /*
    public static void AddDbContexts(IConfiguration config, IServiceCollection services)
    {
      var connectionStringApp = config["Sql:ConnectionStrings:Estore"] ?? throw new InvalidOperationException("Connection string 'EStore' not found.");
      var connectionStringIdentity = config["Sql:ConnectionStrings:Identity"] ?? throw new InvalidOperationException("Connection string 'Identity' not found.");
      //var connectionStringApp = config.GetConnectionString("EStore") ?? throw new InvalidOperationException("Connection string 'EStore' not found.");
      //var connectionStringIdentity = config.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");


      services.AddDatabaseDeveloperPageExceptionFilter();
      bool useInMemoryDatabase = false;


      if (config["useInMemoryDatabase"] is not null and "True")
        useInMemoryDatabase = true;

      if (useInMemoryDatabase)
      {
        services.AddDbContext<EStoreDbContext>(options => options.UseInMemoryDatabase("Cont"));
        services.AddDbContext<EstoreIdentityDbContext>(options => options.UseInMemoryDatabase("Identity"));

        return;
      }

      services.AddDbContext<EstoreIdentityDbContext>(options => options.UseSqlServer(connectionStringIdentity));
      services.AddDbContext<EStoreDbContext>(options => options.UseSqlServer(connectionStringApp));

    }
    */
  }
}
