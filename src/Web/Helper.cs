﻿using EStore.Infra.EF;

using Serilog;

namespace EStore.Web
{
  public sealed class Helper
  {
    public const string TEST_NAME = "TEST_NAME";

    

    public static void SetSeriLog()
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
      //.WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-dd-MM HH:mm:ss:fff}\t[{Level:u3}]\t{Message:lj}\t{NewLine}{Exception}")
      .WriteTo.File("Log.txt",
      outputTemplate: "[{Timestamp:yyyy-dd-MM HH:mm:ss:fff}\t[{Level:u3}]\t{Message:lj}\t{NewLine}{Exception}")
      .CreateLogger();
    }

    public async static void SeedEstoreDb(WebApplication app)
    {
      if (app == null)
        throw new ArgumentNullException("app");
      using (var scope = app.Services.CreateScope())
      {
        var scopedProvider = scope.ServiceProvider;
        try
        {
          Log.Information("Seeding Estore DB");
          var estoreCont = scopedProvider.GetRequiredService<EStoreDbContext>();
          await EstoreContextSeed.Seed(estoreCont);



          //SeedIdentityDB(scopedProvider); //seed identity db

        }
        catch (Exception ex)
        {
          app.Logger.LogError(ex, "An error occurred seeding the DB.");
        }
      }

    }

    public static IServiceScope GetServiceScope(WebApplication app)
    {
      if (app == null)
        throw new ArgumentNullException("app");
      return app.Services.CreateScope();
    }
    public static void LogD()
    {
      
    }



  }//eo cls
}
