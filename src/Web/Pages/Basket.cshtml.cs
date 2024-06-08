using AutoMapper;
using Estore.Core.Extensions;
using Estore.Core.Services;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text.Json;

namespace EStore.Web.Pages
{
  public class BasketModel : PageModel
  {
    private readonly ProductService _productService;
    private readonly BasketService _basketService;
    private readonly IMapper _mapper;
    internal BasketVM BasketVM { get; set; }
    public BasketModel(ProductService productService, BasketService basketService, IMapper mapper)
    {
      _productService = productService;
      _basketService = basketService;
      _mapper = mapper;
    }

    private string? GetBuyerId()
    {
      return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    
    
    public async Task<IActionResult> OnGet()
    {
      var buyerId = GetBuyerId();
      buyerId.GuardNullOrEmpty();

      var basket = await _basketService.GetBasketAsync(buyerId!, false, true);
      BasketVM = _mapper.Map<BasketVM>(basket);
     
      return Partial("_basket", BasketVM);
     

    }


    public async Task<ContentResult> OnGetGetBasketCount()
    {
      var buyerId = GetBuyerId();
      buyerId.GuardNullOrEmpty();

      var basket = await _basketService.GetBasketAsync(buyerId!, true);
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




  }//eo class
}
