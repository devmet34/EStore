using Estore.Core.Interfaces;
using EStore.Infra.EF.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

namespace EStore.WebApi.Controllers
{
  [Route("api/[controller]")]
  
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IIdentityTokenClaimService _identityTokenClaimService;
    //private readonly IConfiguration _config;
    

    public AuthController(SignInManager<AppUser> signInManager, IIdentityTokenClaimService identityTokenClaimService)
    {
      _signInManager = signInManager;
      _identityTokenClaimService = identityTokenClaimService;
      //_config = config;
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username" example="test"></param>
    /// <param name="pass" example="pass"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    [Route("")]
    public async Task<AppLoginResponse> Login(AppLoginModel loginModel)
    {
      if (!ModelState.IsValid)
        throw new ArgumentException();
        
      var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password,false,
        false);
      
      
      if (result.Succeeded)
      {
        
        var token= await _identityTokenClaimService.GetTokenAsync(loginModel.UserName);
        return new AppLoginResponse(true, result.ToString(),token);
        
      }
      //signin failed
      return new AppLoginResponse(false, result.ToString(), null);
      

    }
    
  }
}
