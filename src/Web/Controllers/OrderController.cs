using EStore.App.Services;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    var basketVM = await basketService.GetBasketVMAsync(buyerId);
    basketVM.GuardNull();
    basketVM?.BasketItems.GuardNull();


    return View("checkout", basketVM);


  }


  [HttpPost]
  [Route("OrderController/MakeOrder")]
  public async Task<IActionResult> MakeOrder([FromServices] OrderService orderService)
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
    var orders = await orderService.GetAllOrdersAsync(buyerId);

    return View(orders);


  }//eo class

}