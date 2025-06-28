using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

/// <summary>
/// mc; Mvc.testing.webapplicationfactory. 
/// Creates WebApplicationFactories which runs given entry point (Program.cs). It can be used for integration tests with middlewares, DI services, db etc.  
/// </summary>
public class ProgramFactory
{

    public readonly static WebApplicationFactory<Program> webApplicationFactory = new();

    public readonly static WebApplicationFactory<ProgramApi> webApiApplicationFactory = new();



    public static HttpClient Client => webApplicationFactory.CreateClient();

    public static HttpClient ApiClient => webApiApplicationFactory.CreateClient();


}
