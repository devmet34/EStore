//using AspNetCore;
using AutoMapper;
using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Services;
using EStore.Infra.EF.Identity;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
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
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private const string DEFAULT_SORT = "id";
    

    public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager,  ProductService productService,BasketService basketService, IMapper mapper)
    {
      _logger = logger;
      _signInManager = signInManager;
      _productService = productService;
      _basketService = basketService;
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

      var products= await _productService.GetProductsAsync(sortBy);
      //var productModels=new IEnumerable<ProductModel>();
      
      var productVM=_mapper.Map<IEnumerable< ProductVM>>(products);

      var homeVM = new HomeVM() { Basket = basket, Products = productVM };
      
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



    [HttpPost]
    [Authorize]
    [Route("AddProduct")]
    [ValidateAntiForgeryToken]
    public async Task AddProductAsync( int productId)
    {

      productId.GuardZero();
      productId.GuardNegative();

      var buyerId= User.FindFirstValue(ClaimTypes.NameIdentifier);
      GuardExtensions.GuardNullOrEmpty(buyerId);
      
      await _basketService.AddProductAsync(buyerId!, productId);
      //return RedirectToAction("Index");
    }


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
    [Route("getbasket")]
    public async Task<IActionResult> GetBasketAsync()
    {
      var buyerId=GetBuyerId();
      buyerId.GuardNullOrEmpty();

      var basket=await _basketService.GetBasketAsync(buyerId!,false,true);
      var basketVM=_mapper.Map<BasketVM>(basket);
      _logger.LogError("basketcount:"+basketVM?.BasketItems?.Count());
      return PartialView("_basket", basketVM);

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
