using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Estore.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EStore.Infra.EF.Identity;

public class IdentityTokenClaimService : IIdentityTokenClaimService
{
  private readonly UserManager<AppUser> _userManager;
  private readonly IConfiguration _config;

  public IdentityTokenClaimService(UserManager<AppUser> userManager, IConfiguration config)
  {
    _userManager = userManager;
    _config = config;
  }

  public async Task<string> GetTokenAsync(string userName)
  {
    
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_config["JwtSignKey"] ?? throw new Exception("No signing key"));
    var user = await _userManager.FindByNameAsync(userName);
    if (user == null) throw new Exception($"User: {userName} not found");
    var roles = await _userManager.GetRolesAsync(user);
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

    foreach (var role in roles)
    {
      claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Issuer = "estore",
      Subject = new ClaimsIdentity(claims.ToArray()),
      Expires = DateTime.Now.AddMinutes(1),
      //Expires = DateTime.UtcNow.AddDays(ConstIdent.JWT_TIMEOUT_DAYS),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    //var token = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);
    return token;
    //return tokenHandler.WriteToken(token);
  }
}
