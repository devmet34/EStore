using Estore.Core.Entities;
using Estore.Core.Entities.ValueObjects;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.App.Services;
using Estore.Core.Specs;
using EStore.Infra.EF.Config;
using EStore.Infra.EF.Identity;
using EStore.Infra.EF.Repos;
using EStore.Web;
using EStore.Web.Config;
using EStore.WebApi;
using EStore.WebApi.Extensions;
using EStore.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Extensions.Logging;
using System.Text;
using ILogger = Microsoft.Extensions.Logging.ILogger;

/////////// Api
///////////
///
var builder = WebApplication.CreateBuilder(args);

Helper.SetSeriLog();

//builder.Logging.AddConsole();
builder.Logging.AddSerilog();

//var prod = new Product("sd", null, null, 5, 5);


// Add services to the container.

if (builder.Environment.IsDevelopment())
  ConfigDb.AddDbContexts(builder.Configuration, builder.Services);

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


public partial class Program();