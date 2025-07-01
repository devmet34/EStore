using EStore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections;

namespace EStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IBasketService _basketService;
        public TestController()
        {


        }

        /// <summary>
        /// Api method for testing auth.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getnumbers_auth")]
        public IEnumerable GetNumbers()
        {
            return Enumerable.Range(0, 10).ToArray();
        }

        [HttpGet]
        [Route("getstring")]
        public string GetString()
        {
            Log.Information("test/getstring");
            return "test";
        }

    }
}
