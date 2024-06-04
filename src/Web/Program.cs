
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



// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

//todo console log as well? +
Helper.SetSeriLog();
Log.Information("App starting..");
builder.Logging.AddSerilog();

if (builder.Environment.IsDevelopment())
{
  ConfigDb.AddDbContexts(builder.Configuration, builder.Services);
  //builder.Services.AddDatabaseDeveloperPageExceptionFilter();
  //var connectionString = builder.Configuration.GetConnectionString("EStore") ?? throw new InvalidOperationException("Connection string 'EStore' not found.");
  //var connectionString = builder.Configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");

  //var connectionString = builder.Configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");

  //builder.Services.AddDbContext<Cont>(options => options.UseSqlServer(connectionString));
  //builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppIdentityDbContext>();
  
}

builder.Services.AddIdentity<AppUser, IdentityRole>()
  .AddDefaultUI()
  .AddEntityFrameworkStores<EstoreIdentityDbContext>()
  .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

ConfigRedis.AddRedis(builder);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCoreServices();


var app = builder.Build();
Log.Information("App created...");

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

//Helper.SeedEstoreDb(app);
/*
using (var scope = app.Services.CreateScope())
{
  var scopedProvider = scope.ServiceProvider;
  var estoreCont = scopedProvider.GetRequiredService<EStoreDbContext>();
  await UpdateProducts.Update(estoreCont);
}
*/
  
  async void SeedIdentityDB(IServiceProvider scopedProvider)
{
  var userManager = scopedProvider.GetRequiredService<UserManager<AppUser>>();
  var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
  var identityContext = scopedProvider.GetRequiredService<EstoreIdentityDbContext>();
  await EstoreIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager);

}
app.UseHttpsRedirection();
app.UseStaticFiles();


app.Use( (context,next) => {
  //if (context.Request.QueryString.ToString().Contains("Identit"))
    //throw new Exception("Custom error");


  return next();
  int t = 1;

});


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


app.Run();

 