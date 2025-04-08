using Xunit.Abstractions;

namespace IntegrationTests;
public class DBTest
{
  private ITestOutputHelper _output;


  //private EfRepo<Product> _repo;
  public DBTest(ITestOutputHelper output)
  {
    _output = output;

    //_repo = repo;

  }


}
