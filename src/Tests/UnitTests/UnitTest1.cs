using Xunit.Abstractions;


namespace UnitTests;

public class UnitTest1
{
  ITestOutputHelper _output;
  public UnitTest1(ITestOutputHelper output)
  {
    _output = output;
  }

  [Fact]
  public void Test()
  {
    string? t = "test";

    Helper.LogObjectHash(t);


  }
}