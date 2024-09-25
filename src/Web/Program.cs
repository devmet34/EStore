
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


// Add services to the container.

var builder = WebApplication.CreateBuilder(args);


//Helper.SetSeriLog();
//builder.Logging.AddSerilog();



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
builder.Services.AddRazorPages(options =>
{

});


ConfigRedis.AddRedis(builder);


//builder.Services.AddHostedService<RedisHealthCheckService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCoreServices();


var app = builder.Build();
app?.Logger.LogInformation("App created...");

/*
app.MapGet("/", () =>
Results.File("images/1.png",contentType:"image/png")

) ;
*/

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

}


else
{

  app.UseExceptionHandler("/Home/Error");
  //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}



//await Helper.UpdateProd(app);



//Helper.SeedEstoreDb(app);
/*
using (var scope = app.Services.CreateScope())
{
  var scopedProvider = scope.ServiceProvider;
  var estoreCont = scopedProvider.GetRequiredService<EStoreDbContext>();
  await UpdateProducts.Update(estoreCont);
}


async void SeedIdentityDB(IServiceProvider scopedProvider)
{
  var userManager = scopedProvider.GetRequiredService<UserManager<AppUser>>();
  var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
  var identityContext = scopedProvider.GetRequiredService<EstoreIdentityDbContext>();
  await EstoreIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager);

}
*/


/*
app.Use(async (context, next) => {
  Helper.LogCrit("app.use before next"); 
  await next();
  var logger = context.RequestServices.GetRequiredService<ILogger<object>>();
  Helper.LogCrit("app.use after next");

});
*/

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapGet("filter", async context =>
{
  using var scope=context.RequestServices.CreateScope();
  var prodService= scope.ServiceProvider.GetRequiredService<ProductService>();
  var prods = await prodService.FilterProductsAsync();
  var prods2=await prodService.FilterProductsAsync(true);
});

//mc; map for listing all registered routes
app.MapGet("routes", (IEnumerable<EndpointDataSource> epSources) =>
{
  var res = string.Join("\n", epSources.SelectMany(ep => ep.Endpoints));
  return res;

});


app.UseRouting();


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


app.Run();

