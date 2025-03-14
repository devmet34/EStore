
using EStore.Web;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EStore.Infra.EF.Identity;
using EStore.Infra.EF;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using EStore.Infra.EF.Config;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Mime;
using EStore.Web.Config;
using AutoMapper;
using EStore.Infra.EF.Helpers;
using NuGet.Protocol;
using MC.Logger;
using Microsoft.VisualBasic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing.Text;
using EStore.Web.Middlewares.Custom;
using Web;
using static Web.Constants;
using Estore.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Estore.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.Mvc;
using Estore.Core.Entities.BasketAggregate;


// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

Helper.SetSeriLog();
builder.Logging.AddSerilog();

builder.Services.AddAntiforgery();

//builder.Logging.AddMCLogger(builder.Configuration).AddFileSink(); //mc custom logger
//builder.Services.Configure<MCLoggerOptions>(builder.Configuration.GetSection("MCLoggerOptions"));
/*builder.Logging.AddMCLogger(options => { 
  options.Delimeter = "||||";options.LogLevel = LogLevel.Information; });
*/

//Helper.SetMCLogger();

ConfigDb.AddDbContexts(builder.Configuration, builder.Services);
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<AppUser, IdentityRole>()
  .AddDefaultUI()
  .AddEntityFrameworkStores<EstoreIdentityDbContext>()
  .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
  options.ExpireTimeSpan = TimeSpan.FromHours(applicationCookieTimeoutHours);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//mc, add/register redis stackexchange to di container
ConfigRedis.AddRedis(builder);

//builder.Services.AddHostedService<RedisHealthCheckService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCoreServices(); //mc, registering custom services for the app.

var app = builder.Build();
app?.Logger.LogInformation("App created...");


// Configure the HTTP request pipeline.

//ConfigRedis.PostConfigRedisCacheOptions(app!.Services); //mc, for redis logging.

if (app!.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

}

else
{

  app.UseExceptionHandler("/Home/Error");
  //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


//mc; map for listing all registered routes

app.MapGet("routes", (IEnumerable<EndpointDataSource> epSources) =>
{
  var res = string.Join("\n", epSources.SelectMany(ep => ep.Endpoints));
  return res;

}).RequireAuthorization();


app.Run();

public partial class Program(); //mc, for integration tests

/*
//mc, test debug minimal api with services injected
app.MapGet("testsp", async ( [FromServices] ILogger<Program> logger, [FromServices] IRepo<Basket> repo, HttpContext context) => {
  logger.LogDebug("******in testsp");
  //var q=repo.Query<Basket>();
  var t = await repo.DbSet.FromSql($"exec dbo.test_sp").ToListAsync();
  logger.LogDebug("******testsp end");
  return Results.Ok(t);

});
*/

/* 
//mc, test debug cookie and body alterations
app.Use(async (context, next) => {
  Helper.LogCritical("app.use before next");
  if (context.Request.Path.ToString().Contains("route"))
  {
    Helper.LogCritical(context.Request.Path);
    context.Response.Cookies.Append("test_cookie", "this is test cookie val");
  }
  
  var stream=context.Request.Body;
  var reader=new StreamReader(stream);
  var res=await reader.ReadToEndAsync();
  
  await next();

  Helper.LogCritical("app.use after next");



});
*/