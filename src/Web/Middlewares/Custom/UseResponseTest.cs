﻿namespace EStore.Web.Middlewares.Custom;

public static class UseResponseTest
{
  /// <summary>
  /// mc, Custom middleware, its used to get/test responsebody for debugging.
  /// </summary>
  /// <param name="app"></param>
  /// <returns></returns>
  public static IApplicationBuilder UseResponseBody(this IApplicationBuilder app)
  {
    app.Use(async (context, next) =>
    {
      //string str = "reset";
      var originalBody = context.Response.Body;
      var memStream = new MemoryStream();
      context.Response.Body = memStream;
      Helper.LogCritical("useresponse before next");
      //throw new Exception("exception from useresponse");
      await next();

      

      memStream.Position = 0;
      var strMem = await new StreamReader(memStream).ReadToEndAsync();
      memStream.Position = 0;
      await memStream.CopyToAsync(originalBody);

      var logger=context.RequestServices.GetRequiredService<ILogger<object>>();
      Helper.LogCritical("useresponse after next");
      //var resp=await new StreamReader(context.Response.Body).ReadToEndAsync();

       context.Response.Body=originalBody;
    });
    return app;
  }
}
