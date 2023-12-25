using Serilog;

namespace EStore.Web
{
  public static class Helper
  {
    public const string TEST_NAME = "TEST_NAME";
        
    public static void SetSeriLog()
    {
      Log.Logger = new LoggerConfiguration()
      .WriteTo.File("Log.txt",
      outputTemplate: "[{Timestamp:yyyy-dd-MM HH:mm:ss:fff}\t[{Level:u3}]\t{Message:lj}\t{NewLine}{Exception}")
      .CreateLogger();
    }
  }
}
