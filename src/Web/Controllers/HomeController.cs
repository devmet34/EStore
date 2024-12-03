//using AspNetCore;
using AutoMapper;
using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Models;
using Estore.Core.Services;
using EStore.Infra.EF.Identity;
using EStore.Web.Config;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Web;

namespace EStore.Web.Controllers
{

    public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ProductService _productService;
    private readonly IBasketService _basketService;
    private readonly RedisService _redisService;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private const string DEFAULT_SORT = Constants.DEFAULT_SORT;
    private readonly string cacheProductsKey = Constants.cacheProductsKey;
    
    //mc; separate controllers or razor pages would be better to mitigate di overhead. not using for brevity 
    public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager,  ProductService productService, IBasketService basketService, RedisService redisService, IMapper mapper)
    {
      _logger = logger;
      _signInManager = signInManager;
      _productService = productService;
      _basketService = basketService;
      _redisService = redisService;
      _mapper = mapper;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Test(string id)
    {
      await Task.Delay(1000);
      Thread.Sleep(5000);
      return Ok("test ok");
    }

    private bool IsUserSigned()
    {
      return _signInManager.IsSignedIn(HttpContext.User);
    }

    //mc For home page; get 20 products from redis cache if cached otherwise fetch from db
    public async Task<IActionResult> Index( int page = 1, string sortBy = DEFAULT_SORT, bool? isSuccess=null)
    {
      if (!ModelState.IsValid)
        throw new ArgumentException();
      
      ViewData["success"] = isSuccess;
      Basket? basket = null;
      if (IsUserSigned())
        basket = await GetOrCreateBasketAsync();

      HomeVM? homeVM=null;
      IEnumerable<ProductVM>? cachedProducts=null;
      if (RedisHealthCheckService.IsRedisConnected)
      
      {
        try
        {
          cachedProducts = await _redisService.GetCachedDataAsync<IEnumerable<ProductVM>>(cacheProductsKey);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex.ToString());
        }
      }
     

      if (cachedProducts != null)
        {
          _logger.LogDebug("Fetched products from redis cache");
          homeVM = new HomeVM() { Basket = basket, Products = cachedProducts };
          return View(homeVM);

        }            

      IEnumerable<ProductVM>? productVM = null;
      var products = await _productService.GetProductsPagedAsync(sortBy);
      if (products != null)
      {
        productVM = _mapper.Map<IEnumerable<ProductVM>>(products);
        if (RedisHealthCheckService.IsRedisConnected)
        {
          try
          {
            await _redisService.SetCacheDataAsync<IEnumerable<ProductVM>>(cacheProductsKey, productVM, TimeSpan.FromMinutes(30));
          }
          catch (Exception ex)
          {
            _logger?.LogError(ex.ToString());
          }
        }
      }        

      homeVM = new HomeVM() { Basket = basket, Products = productVM };
      
      return View(homeVM);
    }



    [HttpGet]
    [Route("findproducts")] //todo find products done without paging +
    public async Task<IActionResult> FindProducts(string productName)
    {
      var products = await _productService.FindProductsAsync(productName);
      var productVM = _mapper.Map<IEnumerable<ProductVM>>(products);

      return PartialView("_productcards", productVM);
    }


    [HttpGet]
    [Route("sortproducts")]
    public async Task<IActionResult> SortProducts(string sortBy)
    {
      var products = await _productService.GetProductsPagedAsync( sortBy:sortBy);     
      var productVM = _mapper.Map<IEnumerable<ProductVM>>(products);      

      return PartialView("_productcards", productVM);

    }


    [HttpGet]
    [Route("getproductsbypage")]
    public async Task<IActionResult> GetProductsByPage( int page, string sortBy= DEFAULT_SORT,  string? find = null, FilterModel? filterModel=null)
    {
      if (!ModelState.IsValid)
        throw new ArgumentException();

      
      var products= await _productService.GetProductsOnPageAsync(page, sortBy, find,filterModel);
      var productVM = _mapper.Map<IEnumerable<ProductVM>>(products);
      if (productVM.Count()>=1)
        return PartialView("_productcards", productVM);
      throw new Exception("No more products for page:"+page);
    }



    /*
    [HttpPost]
    [Authorize]
    [Route("SetBasketItem")]
    [ValidateAntiForgeryToken]
    public async Task SetBasketItem(int productId, int qt)
    {

      productId.GuardZero();
      productId.GuardNegative();

      

      var buyerId = GetBuyerId();
      GuardExtensions.GuardNullOrEmpty(buyerId);


      await _basketService.SetBasketItemAsync(buyerId!, productId,qt);
      //return RedirectToAction("Index");
    }
    */

    /*
    [HttpPost]
    [Authorize]
    [Route("RemoveBasketItem")]
    public async Task<IActionResult> RemoveBasketItem([FromBody] int productId)
    {
      productId.GuardZero();
      productId.GuardNegative();

      

      var buyerId = GetBuyerId();
      GuardExtensions.GuardNullOrEmpty(buyerId);

      await _basketService.RemoveBasketItemAsync(buyerId!,productId);
      //return RedirectToAction("getbasket");
      return Ok("Basket item removed");

    }
    


    [HttpGet]
    [Authorize]
    [Route("getbasketcount")]
    public async Task<int> GetBasketCountAsync()
    {
      var buyerId = GetBuyerId();
      buyerId.GuardNullOrEmpty();

      var basket = await _basketService.GetBasketAsync(buyerId!, true);
      basket.GuardNull();
      return basket!.BasketItems.Count;
    }

    */


      private string? GetBuyerId()
    {
      return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      var exceptionHandlerPathFeature =
           HttpContext.Features.Get<IExceptionHandlerPathFeature>();
      _logger.LogCritical("#################:"+exceptionHandlerPathFeature?.Error.Message);
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    async Task<Basket?> GetOrCreateBasketAsync()
    {
      var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (buyerId == null)
        return null;
      return await _basketService.GetOrCreateBasketAsync(buyerId);

        
    }
  }//eo cls
}
