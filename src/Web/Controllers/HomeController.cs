//using AspNetCore;
using AutoMapper;
using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Services;
using EStore.Infra.EF.Identity;
using EStore.Web.Config;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace EStore.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ProductService _productService;
    private readonly BasketService _basketService;
    private readonly RedisService _redisService;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private const string DEFAULT_SORT = "id";
    private readonly string cacheProducts = ":Products";
    
    //mc; separate controllers or razor pages would be better to mitigate di overhead. not using for brevity 
    public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager,  ProductService productService,BasketService basketService, RedisService redisService, IMapper mapper)
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
    public async Task<IActionResult> test(string id)
    {
      _logger.LogError("*********" + id);
      Thread.Sleep(5000);
      return Ok("test ok");
    }

    private bool IsUserSigned()
    {
      return _signInManager.IsSignedIn(HttpContext.User);
    }


    public async Task<IActionResult> Index( int page = 1, string sortBy = DEFAULT_SORT)
    {
      if (!ModelState.IsValid)
        throw new ArgumentException();

     
      Basket? basket = null;
      if (IsUserSigned())
        basket = await GetOrCreateBasketAsync();

      HomeVM? homeVM=null;
      IEnumerable<ProductVM>? cachedProducts=null;
      if (RedisHealthCheckService.isRedisConnected)
      {
        try
        {
          cachedProducts = await _redisService.GetCachedDataAsync<IEnumerable<ProductVM>>(cacheProducts);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex.ToString());
        }
      }

      if (cachedProducts != null)
        {
          _logger.LogDebug("Fetched products from cache");
          homeVM = new HomeVM() { Basket = basket, Products = cachedProducts };
          return View(homeVM);

        }            

      IEnumerable<ProductVM>? productVM = null;
      var products = await _productService.GetProductsAsync(sortBy);
      if (products != null)
      {
        productVM = _mapper.Map<IEnumerable<ProductVM>>(products);
        if (RedisHealthCheckService.isRedisConnected)
        {
          try
          {
            await _redisService.SetCacheDataAsync<IEnumerable<ProductVM>>(cacheProducts, productVM, TimeSpan.FromMinutes(30));
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
      var products = await _productService.GetProductsAsync( sortBy:sortBy);     
      var productVM = _mapper.Map<IEnumerable<ProductVM>>(products);      

      return PartialView("_productcards", productVM);

    }




    [HttpGet]
    [Route("getproductsbypage")]
    public async Task<IActionResult> GetProductsByPage( int page, string sortBy= DEFAULT_SORT, string? find = null)
    {     
      if (!ModelState.IsValid)
        throw new ArgumentException();
      _logger.LogError("sort/find:" + sortBy + '/' + find);
      var products= await _productService.GetProductsOnPageAsync(page, sortBy, find);
      var productVM = _mapper.Map<IEnumerable<ProductVM>>(products);
      if (productVM.Count()>1)
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
