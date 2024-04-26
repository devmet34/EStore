using EStore.Infra.EF.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Security.Claims;


namespace EStore.WebApi.Middlewares {

  public class TestMiddleware
  {
    RequestDelegate _next;
    

    //ILogger _logger;
    public TestMiddleware(RequestDelegate next) {

      _next = next;
      
      
      //_logger = logger.CreateLogger<TestMiddleware>(); 
    }

    public async Task InvokeAsync(HttpContext context)
    {

      /*
      var scopedProvider = context.RequestServices;
      var handlerProvider = scopedProvider.GetService<IAuthenticationHandlerProvider>();
      var authService = scopedProvider.GetServices<IAuthenticationService>();
      var jwtOptions = scopedProvider.GetServices<Microsoft.Extensions.Options.IConfigureOptions<JwtBearerOptions>>();
      var jwpPost = scopedProvider.GetServices<Microsoft.Extensions.Options.IPostConfigureOptions<JwtBearerOptions>>();
      var jwtHandler = scopedProvider.GetService<JwtBearerHandler>();
      Log.Information("testmiddleware");
      /*
      var claims = new[] { new Claim("name", "YourName"), new Claim(ClaimTypes.Role, "Admin") };
      var identity = new ClaimsIdentity(claims, "JWT");
      context.User= new ClaimsPrincipal(identity);
      */
      //throw new Exception("test");
      await _next(context);

    }
  }

}