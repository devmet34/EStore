

using EStore.Infra.EF;

namespace ConsoleUnitTests;

internal class Program
{
  static async Task Main(string[] args)
  {
    string t = "abcd";
    
    DbContextTest context = new();
    await context.Test();

  }
}
