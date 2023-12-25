
using EStore.Web;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EStore.Infra.EF.Identity;




Helper.SetSeriLog();
Log.Information("This is test");

//test();

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
  var connectionString = builder.Configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");

  builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString));
  builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppIdentityDbContext>();
  builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
  // app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

//AppIdentityDbContext appIdentityDbContext = new(new());

using (var scope = app.Services.CreateScope())
{
  var scopedProvider = scope.ServiceProvider;

  var context = scopedProvider.GetRequiredService<AppIdentityDbContext>();
  context.Database.Migrate();
  int g = 1;
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

 void test() {
 
  
  
}