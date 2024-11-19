
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
using Estore.Core.Services;
using MC.Logger;
using Microsoft.VisualBasic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing.Text;
using EStore.Web.Middlewares.Custom;
using Web;
using static Web.Constants;
using Estore.Core.Entities;
using Microsoft.AspNetCore.Authorization;


// Add services to the container.

var builder = WebApplication.CreateBuilder(args);


//Helper.SetSeriLog();
//builder.Logging.AddSerilog();

builder.Services.AddAntiforgery();

builder.Logging.AddMCLogger(builder.Configuration)
 .AddFileSink();
//builder.Services.Configure<MCLoggerOptions>(builder.Configuration.GetSection("MCLoggerOptions"));
/*builder.Logging.AddMCLogger(options => { 
  options.Delimeter = "||||";options.LogLevel = LogLevel.Information; });
*/


//Helper.SetMCLogger();



ConfigDb.AddDbContexts(builder.Configuration, builder.Services);
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//var connectionString = builder.Configuration.GetConnectionString("EStore") ?? throw new InvalidOperationException("Connection string 'EStore' not found.");
//var connectionString = builder.Configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");

//var connectionString = builder.Configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");

//builder.Services.AddDbContext<Cont>(options => options.UseSqlServer(connectionString));
//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppIdentityDbContext>();



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


ConfigRedis.AddRedis(builder);

//builder.Services.AddHostedService<RedisHealthCheckService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCoreServices();



var app = builder.Build();
app?.Logger.LogInformation("App created...");


// Configure the HTTP request pipeline.

app.UseExceptionHandler("/Home/Error");
//The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

if (app.Environment.IsDevelopment())
{
  //app.UseDeveloperExceptionPage();

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



app.Use(async (context, next) => {
  Helper.LogCritical("app.use before next");
  if (context.Request.Path.ToString().Contains("route"))
  {
    Helper.LogCritical(context.Request.Path);
    context.Response.Cookies.Append("test_cookie", "this is test cookie val");
  }
  /*
  var stream=context.Request.Body;
  var reader=new StreamReader(stream);
  var res=await reader.ReadToEndAsync();
  */
  await next();

  Helper.LogCritical("app.use after next");



});

app.MapGet("filter", async context =>
{
  using var scope = context.RequestServices.CreateScope();
  var prodService = scope.ServiceProvider.GetRequiredService<ProductService>();
  
});

//mc; map for listing all registered routes

app.MapGet("routes", (IEnumerable<EndpointDataSource> epSources) =>
{
  var res = string.Join("\n", epSources.SelectMany(ep => ep.Endpoints));
  return res;

}).RequireAuthorization();


app.Run();

