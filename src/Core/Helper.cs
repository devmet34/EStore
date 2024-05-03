

using System.Runtime.CompilerServices;

namespace EStore.Core;

public sealed class Helper
{

  public static void LogObjectHash(object obj, [CallerArgumentExpression("obj")] string? paramName = null)
  {
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.WriteLine($"****** hash of {paramName}:" + obj.GetHashCode());
  }




}//eo cls
