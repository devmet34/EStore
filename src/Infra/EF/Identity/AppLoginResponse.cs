namespace EStore.Infra.EF.Identity
{
  public class AppLoginResponse
  {
    public bool IsSuccess { get; set; } = false;
    public string Msg { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public AppLoginResponse(bool isSuccess, string msg, string token)
    {
      IsSuccess = isSuccess;
      Msg = msg;
      Token = token;

    }

  }
}
