namespace EStore.Infra.EF.Identity
{
  public class AppLoginModel
  {
    public string? UserName { get; init; }
    public string? Password { get; init; }

    public AppLoginModel(string userName, string password)
    {
      UserName = userName;
      Password = password;
    }

  }
}
