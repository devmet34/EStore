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
    private readonly BasketService basketService;
    private readonly OrderService orderService;
   

  
    public IEnumerable< BasketItem> BasketItems { get; set; }
    public CheckoutModel(BasketService basketService, OrderService orderService) {
      this.basketService= basketService;
      this.orderService= orderService;
     
    }
    public async Task OnGet()
    {
      var buyerId = Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
      var basket=await basketService.GetBasketAsync(buyerId);
      basket!.BasketItems.GuardNull();
      BasketItems=basket.BasketItems;
     

    }
  }
}
