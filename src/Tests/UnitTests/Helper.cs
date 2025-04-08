using System.Runtime.CompilerServices;

namespace UnitTests;
public class Helper
{
  private static Random random = new Random();

  public static int GetRandomInt(int start = 0, int end = 999999999)
  {
    return random.Next(start, end);
  }
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
