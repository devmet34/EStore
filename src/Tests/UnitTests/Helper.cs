using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;
public class Helper
{
  public static void Test(string str)
  {

  }

  public static void LogObjectHash(object obj, [CallerArgumentExpression("obj")] string? paramName = null)
  {
    Console.WriteLine($"****** hash of {paramName}:" + obj.GetHashCode());
  }

  public static void Test()
  {

  }
}
