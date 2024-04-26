using EStore.Infra.EF;
using EStore.Infra.EF.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Infra.EF.Config
{
    public static class ConfigDb
    {
        public static void AddDbContexts(IConfiguration config, IServiceCollection services)
        {

            var connectionStringApp = config.GetConnectionString("EStore") ?? throw new InvalidOperationException("Connection string 'EStore' not found.");
            var connectionStringIdentity = config.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");


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

            services.AddDbContext<EstoreIdentityDbContext>(options => options.UseSqlServer(config.GetConnectionString("Identity")));
            services.AddDbContext<EStoreDbContext>(options => options.UseSqlServer(config.GetConnectionString("EStore")));
            
        }
    }
}
