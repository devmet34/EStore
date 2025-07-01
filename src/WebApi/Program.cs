using EStore.Core.Interfaces;
using EStore.Infra.EF.Config;
using EStore.Infra.EF.Identity;
using EStore.Web;
using EStore.Web.Config;
using EStore.WebApi.Extensions;
using EStore.WebApi.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
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

builder.Services.AddScoped<IIdentityTokenClaimService, EStore.App.Services.IdentityTokenClaimService>();
builder.Services.AddIdentity<AppUser, IdentityRole>()
  .AddEntityFrameworkStores<EstoreIdentityDbContext>()
  .AddDefaultTokenProviders();

ConfigRedis.AddRedis(builder);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCoreServices();

/* test basic auth
builder.Services.AddAuthentication(opt =>opt.DefaultAuthenticateScheme=AuthenticationSchemes.Basic);
*/

//custom extension for adding authentications like jwt.
builder.Services.AddAuthentications(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    

});



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


public partial class ProgramApi(); //mc, for integration tests