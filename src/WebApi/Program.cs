using EStore.Core.Interfaces;
using EStore.Infra.EF.Config;
using EStore.Infra.EF.Identity;
using EStore.Web;
using EStore.Web.Config;
using EStore.WebApi.Extensions;
using EStore.WebApi.Middlewares;
using Microsoft.AspNetCore.Identity;
using Serilog;

/////////// Api
///////////
///
var builder = WebApplication.CreateBuilder(args);

Helper.SetSeriLog();

//builder.Logging.AddConsole();
builder.Logging.AddSerilog();

// Add services to the container.

ConfigDb.AddDBContexts(builder);

builder.Services.AddScoped<IIdentityTokenClaimService, IdentityTokenClaimService>();
builder.Services.AddIdentity<AppUser, IdentityRole>()
  .AddEntityFrameworkStores<EstoreIdentityDbContext>()
  .AddDefaultTokenProviders();

ConfigRedis.AddRedis(builder);
builder.Services.AddCoreServices();

/* test basic auth
builder.Services.AddAuthentication(opt =>opt.DefaultAuthenticateScheme=AuthenticationSchemes.Basic);
*/

//custom extension for adding authentications like jwt.
builder.Services.AddAuthentications(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

//Debug4Scope();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<TestMiddleware>();

app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();


//public partial class Program();