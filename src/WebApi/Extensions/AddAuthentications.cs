using EStore.Infra.EF.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EStore.WebApi.Extensions;

public static class AddAuthentication
{
  public static IServiceCollection AddAuthentications(this IServiceCollection services, IConfiguration config)
  {
    //var key = Encoding.ASCII.GetBytes(ConstIdent.JWT_SIGN_KEY);
    var key = Encoding.ASCII.GetBytes(config["JwtSignKey"] ?? throw new Exception("No signing key"));


    services.AddAuthentication(config =>
    {
      config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    //.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

    .AddJwtBearer(config =>
    {

      config.SaveToken = false;
      config.TokenValidationParameters = new TokenValidationParameters
      {
        ValidIssuer = "estore",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = false
      };
    });

    return services;
  }

}//eo cls
