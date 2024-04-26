using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Estore.Core.Extensions;
public static class GuardExtensions
{
  public static void GuardNull<T>(this T? value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value == null)
      throw new ArgumentNullException(paramName, $"{paramName} cant be null");
  }
  
  public static void GuardZero(this int value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value == 0) 
      throw new ArgumentException($"{paramName} cant be zero");    
    
  }

  public static void GuardZeroOrNegative(this decimal value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value <= 0)
      throw new ArgumentException($"{paramName} cant be zero or negative");

  }

  public static void GuardNegative(this decimal value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value < 0)
      throw new ArgumentException($"{paramName} cant be negative");

  }

  public static void GuardNegative(this int value, [CallerArgumentExpression("value")] string? paramName = null)
  {
    if (value < 0)
      throw new ArgumentException($"{paramName} cant be negative");

  }

  public static string GuardNullOrEmpty( this string? value, [CallerArgumentExpression("value")] string? paramName=null)
  {
    if (string.IsNullOrEmpty(value))
      throw new ArgumentException($"{paramName} cant be null or empty");
    return value;

  } 
}
