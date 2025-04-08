using EStore.Infra.EF.Identity;
using NuGet.Protocol;
using System.Text.Json;

namespace IntegrationTests;
[TestClass]
public class AuthControllerTest
{

  string userName = Constants.DEFAULT_USERNAME;
  string pass = Constants.DEFAULT_PASS;

  string uri = "api/auth";

  [TestMethod]
  public async Task TestLogin()
  {

    var loginModel = new AppLoginModel(userName, pass);

    var client = ProgramFactory.Client;

    var resp = await client.GetAsync("api/test");

    var jsonContent = new StringContent(JsonSerializer.Serialize(loginModel), null, "application/json");


    var response = await client.PostAsync(uri, jsonContent);
    var stringResponse = await response.Content.ReadAsStringAsync();

    var loginResponse = stringResponse.FromJson<AppLoginResponse>();
    //Assert.IsNotNull(resp);
  }
}
