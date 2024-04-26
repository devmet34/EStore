using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace WebApi_Integration;
public class Helper4Tests
{
  public static StringContent GetJsonContent(string content,Encoding? encoding,string? type)
  {
    return new StringContent(JsonSerializer.Serialize(content), null, "application/json");
  }
}
