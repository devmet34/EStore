using Microsoft.AspNetCore.Mvc.Testing;



namespace IntegrationTests;


public class ProgramFactory
{
  /// <summary>
  /// mc; Mvc.testing.webapplicationfactory. 
  /// This runs program class in web application. It can be used for integration tests like middlewares, services, db etc.  
  /// </summary>
  public readonly static WebApplicationFactory<Program> webApplicationFactory = new();

  //
  public static HttpClient Client => webApplicationFactory.CreateClient();


}
