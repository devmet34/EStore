﻿using EStore.App.Services;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using EStore.Infra.EF;
using EStore.Infra.EF.Helpers;
using NuGet.Protocol;
using Serilog;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Policy;

namespace EStore.Web
{
  public sealed class Helper
  {
    public const string TEST_NAME = "TEST_NAME";

    //mc; check if 2 given objects are equal by json serializing. 
    public static bool AreObjectsEqual(object obj, object obj2)
    {
      return obj.ToJson()==obj2.ToJson();
    }
    public static void LogObjectHash(object obj, [CallerArgumentExpression("obj")] string? paramName = null)
    {
      Console.WriteLine($"****** hash of {paramName}:" + obj.GetHashCode());
    }

    public static void SetSeriLog()
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
      //.WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-dd-MM HH:mm:ss:fff}\t[{Level:u3}]\t{Message:lj}\t{NewLine}{Exception}")
      .WriteTo.File("Log.txt",
      outputTemplate: "[{Timestamp:yyyy-dd-MM HH:mm:ss:fff}\t[{Level:u3}]\t{SourceContext}\t{Message:lj}\t{NewLine}{Exception}")
      
      
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

    public static List<ServiceDescriptor> ListServicesByKey(WebApplicationBuilder builder, string pattern)
    {
      pattern.GuardNullOrEmpty();

      return builder.Services.Where(s => s.ServiceType.FullName!.Contains(pattern, StringComparison.OrdinalIgnoreCase)).ToList();

    }

    internal static async Task UpdateProd(WebApplication app)
    {
      using var scope = app.Services.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<EStoreDbContext>();
      await UpdateProducts.UpdateProductsBatch(dbContext);
    }

    public static IServiceScope GetServiceScope(WebApplication app)
    {
      if (app == null)
        throw new ArgumentNullException("app");
      return app.Services.CreateScope();
    }

    public static string? GetUserId(ClaimsPrincipal user )
    {
      return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static IBasketService GetBasketService(HttpContext context)
    {
      context.GuardNull();
      return context.RequestServices.GetRequiredService<BasketDBService>();
        
    }
    
    public static void LogD()
    {
      
    }

    public static void LogCritical(string str)
    {
      Console.BackgroundColor = ConsoleColor.Red;
      var date=DateTime.Now;
      Console.WriteLine(date+"**********"+str);

    }



  }//eo cls
}
