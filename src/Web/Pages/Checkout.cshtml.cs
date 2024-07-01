using AutoMapper;
using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Services;
using EStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EStore.Web.Pages
{
  public class CheckoutModel : PageModel
  {
    private readonly BasketService _basketService;
    private readonly OrderService _orderService;
   

    public Basket Basket { get; set; }
    //public IEnumerable< BasketItem> BasketItems { get; set; }
    public CheckoutModel(BasketService basketService, OrderService orderService) {
      this._basketService= basketService;
      this._orderService= orderService;
      
    }
    public async Task OnGet()
    {
      var buyerId = Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
      var basket=await _basketService.GetBasketAsync(buyerId,true,true);
      basket!.BasketItems.GuardNull();
      //BasketItems=basket.BasketItems;
      Basket = basket;
     

    }

    public async Task<IActionResult> OnPostMakeOrder()
    {
      
      var buyerId = GetBuyerId();
      
      await _orderService.CreateOrderAsync(buyerId);
      return RedirectToAction("index", "home");
    }

    private string GetBuyerId()
    {
      return Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
    }
  }
}
