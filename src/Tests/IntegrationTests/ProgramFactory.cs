using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



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
