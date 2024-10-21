namespace EStore.Web.Middlewares.Custom;

public class TestMiddleware
{
  private readonly ILogger _logger;
  private readonly IConfiguration _configuration;
  private readonly RequestDelegate _next;
  public TestMiddleware(ILogger<TestMiddleware> logger,IConfiguration configuration,RequestDelegate next) {
    _logger = logger;
    _configuration = configuration;
    _next = next;  
  }

  public async Task Invoke(HttpContext context) 
  {
    Helper.LogCritical("testmiddleware before next");
    await _next(context);
    Helper.LogCritical("testmiddleware after next");
  }


}

