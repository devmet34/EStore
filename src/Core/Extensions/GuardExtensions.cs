using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace EStore.Core.Extensions;
public static class GuardExtensions
{
  /// <summary>
  /// Throw ArgumentNullException if given object is null.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <param name="paramName"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public static void GuardNull<T>(this T? value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value == null)
      throw new ArgumentNullException(paramName, $"{paramName} cant be null");
  }
  
  public static int GuardZero(this int value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value == 0) 
      throw new ArgumentException($"{paramName} cant be zero");
    return value;
  }

  public static decimal GuardZeroOrNegative(this decimal value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value <= 0)
      throw new ArgumentException($"{paramName} cant be zero or negative");
    return value;
  }

  public static decimal GuardNegative(this decimal value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value < 0)
      throw new ArgumentException($"{paramName} cant be negative");
    return value;
  }

  public static int GuardNegative(this int value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value < 0)
      throw new ArgumentException($"{paramName} cant be negative");
    return value;
  }

  public static string GuardNullOrEmpty( this string? value, [CallerArgumentExpression("value")] string? paramName=null)
  {
    if (string.IsNullOrEmpty(value))
      throw new ArgumentException($"{paramName} cant be null or empty");
    return value;

  } 
}
