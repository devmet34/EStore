﻿using Estore.Core.Interfaces;
using Estore.Core.Services;
using Estore.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace EStore.Web.Controllers;

[Authorize]

public class OrderController:Controller
{


  [HttpGet]
  [Route("OrderController/GetCheckOut")]
  public async Task<IActionResult> GetCheckOut([FromServices] BasketService basketService)
  {
    var buyerId = Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
    var basket = await basketService.GetBasketAsync(buyerId, true, true);
    basket.GuardNull();
    basket?.BasketItems.GuardNull();

    var basketVM = new BasketVM(basket!.BasketItems,basket.TotalPrice);

    return View("checkout",basketVM);
    
    
  }


  [HttpPost]
  [Route("OrderController/MakeOrder")]
  public async Task<IActionResult> MakeOrder([FromServices] OrderService orderService)
  {

    var buyerId = Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));

    await orderService.CreateOrderAsync(buyerId);
    return RedirectToAction("index", "home");
  }




}//eo class
