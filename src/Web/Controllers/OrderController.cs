using Estore.Core.Interfaces;
using Estore.Core.Services;
using Estore.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace EStore.Web.Controllers;

[Authorize]
public class OrderController : Controller
{
  private readonly ILogger<OrderController> _logger;

  public OrderController(ILogger<OrderController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  [Route("OrderController/GetCheckOut")]
  public async Task<IActionResult> GetCheckOut([FromServices] IBasketService basketService)
  {
    var buyerId = Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
    var basket = await basketService.GetBasketAsync(buyerId);
    basket.GuardNull();
    basket?.BasketItems.GuardNull();

    var basketVM = new BasketVM(basket!.BasketItems, basket.TotalPrice);

    return View("checkout", basketVM);


  }


  [HttpPost]
  [Route("OrderController/MakeOrder")]
  public async Task<IActionResult> MakeOrder([FromServices] OrderService orderService, [FromServices] ProductService productService)
  {
    var buyerId = Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));

    try { await orderService.CreateOrderAsync(buyerId); }
    catch (Exception ex)
    {
      _logger.LogError(ex.Message);
      throw new Exception(ex.Message);
    }

    return RedirectToAction("index", "home", new { isSuccess = true });
  }


  [HttpGet]
  [Route("OrderController/Orders")]
  [Route("Orders")]
  public async Task<IActionResult> Orders([FromServices] OrderService orderService)
  {
    var buyerId = Helper.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
    var orders = await orderService.GetAllOrders(buyerId);

    return View(orders);


  }//eo class

}