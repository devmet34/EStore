using AutoMapper;
using EStore.App.Services;
using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text.Json;

/*
namespace EStore.Web.Pages
{
  [Authorize]
  public class BasketModel : PageModel
  {
    private readonly ProductService _productService;
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketModel> _logger;
    private readonly IMapper _mapper;
    //private readonly BasketServiceFactory _basketServiceFactory;
    internal BasketVM? BasketVM { get; set; }
    public BasketModel(ProductService productService, IBasketService basketService, IMapper mapper, ILogger<BasketModel> logger)
    {      
      _productService = productService;
      _basketService = basketService;
      _mapper = mapper;
      _logger = logger;
      

    }
    
    private string? GetBuyerId()
    {      
      return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    
    
    public async Task<IActionResult> OnGet()
    {
      var buyerId = GetBuyerId();
      buyerId.GuardNullOrEmpty();
      var basket= await _basketService.GetBasketAsync(buyerId!);
      
      //var basket = await _basketService.GetBasketAsync(buyerId!, false, true);

      BasketVM = _mapper.Map<BasketVM>(basket);
     
      return Partial("_basket", BasketVM);
     

    }

    
    public async Task<ContentResult> OnGetGetBasketCount()
    {
      var buyerId = GetBuyerId();
      buyerId.GuardNullOrEmpty();

      //var basket = await _basketService.GetBasketAsync(buyerId!, true);
      var basket = await _basketService.GetBasketAsync(buyerId!);
      basket.GuardNull();
      return Content( basket!.BasketItems.Count.ToString());
    }


    public async Task<IActionResult> OnPostSetBasketItem(int productId, int qt)
    {

      productId.GuardZero();
      productId.GuardNegative();

      //throw new NotImplementedException();

      var buyerId = GetBuyerId();
      GuardExtensions.GuardNullOrEmpty(buyerId);


      await _basketService.SetBasketItemAsync(buyerId!, productId, qt);
      return new OkResult();
      //return RedirectToAction("Index");
    }



    public async Task<IActionResult> OnPostRemoveBasketItem([FromBody] int productId)
    {
      productId.GuardZero();
      productId.GuardNegative();



      var buyerId = GetBuyerId();
      GuardExtensions.GuardNullOrEmpty(buyerId);

      await _basketService.RemoveBasketItemAsync(buyerId!, productId);
      return new OkResult();
      //return RedirectToAction("getbasket");
      //return  "Basket item removed";

    }

    public async Task<IActionResult> OnPostRemoveBasket()
    {
      _logger.LogWarning("Removing basket");
      var buyerId = GetBuyerId();
      buyerId.GuardNullOrEmpty();

      var basket=await _basketService.GetBasketAsync(buyerId!);
      basket.GuardNull();

      await _basketService.RemoveBasketAsync(basket!);

      return RedirectToAction("index", "home"); 

    }



  }//eo class
}


*/