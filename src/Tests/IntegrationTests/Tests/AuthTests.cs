using EStore.Infra.EF.Identity;
using NuGet.Protocol;
using System.Text.Json;

namespace IntegrationTests.Tests;
[TestClass]
public class AuthTests
{

    string userName = Constants.DEFAULT_USERNAME;
    string pass = Constants.DEFAULT_PASS;

    string authApiLoginUri = "api/auth/login";
    /// <summary>
    /// mc, Test Api login (local Identity), also tests JWT token generation.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task TestApiLoginJwt()
    {

        var loginModel = new AppLoginModel(userName, pass);

        var client = ProgramFactory.ApiClient;

        var httpContent = new StringContent(JsonSerializer.Serialize(loginModel), null, "application/json");

        var response = await client.PostAsync(authApiLoginUri, httpContent);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var loginResponse = stringResponse.FromJson<AppLoginResponse>();

        Assert.IsTrue(response?.IsSuccessStatusCode);
        Assert.IsNotNull(loginResponse?.Token);
    }
}
