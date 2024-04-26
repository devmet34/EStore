using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WebApi_Integration;


public class ProgramFactory
{
  public readonly static WebApplicationFactory<Program> webApplicationFactory = new();
  
  //
  public static HttpClient Client => webApplicationFactory.CreateClient();
  
  
}
