using EStore.Core.Interfaces;
using EStore.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public TestController(IBasketService basketService) {
      _basketService = basketService;
      
    }

    [HttpGet]
    [Route("test")]
    public void Test()
    {
      
      
      
      
    }


    [Authorize]
    [HttpGet]
    [Route("getnumbers")]
    public IEnumerable GetNumbers()
    {
      return  Enumerable.Range(0, 10);
    }

    [HttpGet]
    [Route("/")]
    [Route("getstring")]
    public string GetString()
    {
      Log.Information("test/getstring");
      return "test";
    }
    
  }
}
