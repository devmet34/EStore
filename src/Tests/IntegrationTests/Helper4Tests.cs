using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using Xunit.Sdk;

namespace IntegrationTests;
public class Helper4Tests
{
  public static StringContent GetJsonContent(string content, Encoding? encoding, string? type)
  {
    return new StringContent(JsonSerializer.Serialize(content), null, "application/json");
  }

  public static void WriteOutput(string msg)
  {
    var output = new TestOutputHelper();
    output.WriteLine(msg);
  }

  public static IServiceScope GetServiceScope()
  {
    var app = ProgramFactory.webApplicationFactory;
    return app.Services.CreateScope();
  }
}
