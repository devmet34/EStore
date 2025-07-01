using EStore.Core.Interfaces;
using EStore.Infra.EF.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<AppLoginResponse> Login(AppLoginModel loginModel)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();

            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName!, loginModel.Password!, false,
              false);


            if (result.Succeeded)
            {

                var token = await _identityTokenClaimService.GetTokenAsync(loginModel.UserName!);
                return new AppLoginResponse(true, result.ToString(), token);

            }
            //signin failed
            return new AppLoginResponse(false, result.ToString(), default);


        }

        [HttpGet]
        [Route("LoginWithDefaultUser")]
        public async Task<AppLoginResponse> LoginWithDefaultUser()
        {
            string DEFAULT_USERNAME = "user1@estore.com";
            string DEFAULT_PASS = "Pass@Word1";

            var result = await _signInManager.PasswordSignInAsync(DEFAULT_USERNAME, DEFAULT_PASS, false,
                  false);


            if (result.Succeeded)
            {

                var token = await _identityTokenClaimService.GetTokenAsync(DEFAULT_USERNAME);
                return new AppLoginResponse(true, result.ToString(), token);

            }
            //signin failed
            return new AppLoginResponse(false, result.ToString(), default);


        }

    }
}
