using System.Net;

namespace EStore.WebApi.Middlewares;

public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;

  public ExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (Exception ex)
    {
      httpContext.Response.ContentType = "application/json";


      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      await httpContext.Response.WriteAsync(ex.Message);
    }
  }

  
}
