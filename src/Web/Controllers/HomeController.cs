//using AspNetCore;
using AutoMapper;
using Estore.App.Services;
using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Models;
using EStore.Infra.EF.Identity;
using EStore.Web.Config;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.ContentModel;

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
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private const string DEFAULT_SORT = Constants.DEFAULT_SORT;
    private readonly string cacheProductsKey = Constants.cacheProductsKey;
    
    
    

    //mc; separate controllers or razor pages would be better to mitigate di overhead. not using for brevity 
    public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager, ProductService productService, IBasketService basketService, RedisService redisService, IMapper mapper)
    {
      _logger = logger;
      _signInManager = signInManager;
      _productService = productService;
      _basketService = basketService;
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
    /// <summary>
    /// mc, Home page; get paged (default 20) products from db.
    /// isSuccess bool is used to show success toast msg. Controllers may redirect to here with success msg. 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="sortBy"></param>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    //
    public async Task<IActionResult> Index(int page = 1, string sortBy = DEFAULT_SORT, bool? isSuccess=null)
    {
      if (!ModelState.IsValid)
        throw new ArgumentException();
      
      ViewData["success"] = isSuccess;     
      Basket? basket = null;
      
      if (IsUserSigned())
      {
        basket = await GetOrCreateBasketAsync();
      }
        
      IEnumerable<ProductVM>? productVM = null;
      var products = await _productService.GetProductsPagedAsync(sortBy);
      //if (products != null)      
       // productVM = _mapper.Map<IEnumerable<ProductVM>>(products);         

      var homeVM = new HomeVM() { Basket = basket, Products = products };
      
      return View(homeVM);
    }



    [HttpGet]
    [Route("findproducts")] //todo find products done without paging +
    public async Task<IActionResult> FindProducts(string productName)
    {
      var products = await _productService.FindProductsAsync(productName);
      

      return PartialView("_productcards", products);
    }


    [HttpGet]
    [Route("sortproducts")]
    public async Task<IActionResult> SortProducts(string sortBy)
    {
      var products = await _productService.GetProductsPagedAsync( sortBy:sortBy);     
           

      return PartialView("_productcards", products);

    }


    [HttpGet]
    [Route("getproductsbypage")]
    public async Task<IActionResult> GetProductsByPage( int page, string sortBy= DEFAULT_SORT,  string? find = null, FilterModel? filterModel=null)
    {
      if (!ModelState.IsValid)
        throw new ArgumentException();

      
      var products= await _productService.GetProductsOnPageAsync(page, sortBy, find,filterModel);
      
      if (products?.Count()>=1)
        return PartialView("_productcards", products);
      throw new Exception("No more products for page:"+page);
    }


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
