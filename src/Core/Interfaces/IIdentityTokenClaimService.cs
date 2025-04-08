namespace EStore.Core.Interfaces;
public interface IIdentityTokenClaimService
{
  Task<string> GetTokenAsync(string userName);

}
