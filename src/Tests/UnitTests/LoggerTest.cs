using MC.Logger;

namespace UnitTests;
public class LoggerTest
{
  private MCLoggerProxy loggerProxy;
  public LoggerTest()
  {
    MCLoggerExtensions.AddFileSink();
    loggerProxy = new MCLoggerProxy();
  }

  [Fact]
  public void Test()
  {
    int n = 10;
    for (int i = 0; i < n; i++)
    {
      loggerProxy.Log("This is a test log msg from unit test " + i);
    }
    Thread.Sleep(100);
  }
}
