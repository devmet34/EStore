namespace EStore.Web.Middlewares.Custom;

public class TestMiddleware2
{
  private readonly ILogger _logger;
  private readonly IConfiguration _configuration;
  private readonly RequestDelegate _next;
  public TestMiddleware2(ILogger<TestMiddleware2> logger,IConfiguration configuration,RequestDelegate next) {
    _logger = logger;
    _configuration = configuration;
    _next = next;  
  }

  public async Task Invoke(HttpContext context) 
  {
    Helper.LogCrit("testmiddleware2 before next");
    await _next(context);
    Helper.LogCrit("testmiddleware2 after next");
  }


}

