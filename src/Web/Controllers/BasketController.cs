using AutoMapper;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Services;
using EStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Controllers;


[Authorize]


public class BasketController:Controller
{

 
  private readonly IBasketService _basketService;
  private readonly ILogger<BasketController> _logger;
  private readonly IMapper _mapper;

  public BasketController(IBasketService basketService, ILogger<BasketController> logger, IMapper mapper)
  {  
    _basketService = basketService;
    _logger = logger;
    _mapper = mapper;
  }


  public async Task<IActionResult> GetBasket()
  {
    var buyerId = GetBuyerId();
    buyerId.GuardNullOrEmpty();
    var basket = await _basketService.GetBasketAsync(buyerId!);

    //var basket = await _basketService.GetBasketAsync(buyerId!, false, true);

    BasketVM basketVM = _mapper.Map<BasketVM>(basket);

    return PartialView("_basket", basketVM);


  }

  private string? GetBuyerId()
  {
    return Helper.GetUserId(User);
  }


  public async Task<ContentResult> GetBasketCount()
  {
    var buyerId = GetBuyerId();
    buyerId.GuardNullOrEmpty();

    //var basket = await _basketService.GetBasketAsync(buyerId!, true);
    var basket = await _basketService.GetBasketAsync(buyerId!);
    basket.GuardNull();
    return Content(basket!.BasketItems.Count.ToString());
  }

  [HttpPost]
  public async Task<IActionResult> SetBasketItem(int productId, int qt)
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


  [HttpPost]
  public async Task<IActionResult> RemoveBasketItem([FromBody] int productId)
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

  [HttpPost]
  public async Task<IActionResult> RemoveBasket()
  {
    _logger.LogWarning("Removing basket");
    var buyerId = GetBuyerId();
    buyerId.GuardNullOrEmpty();

    var basket = await _basketService.GetBasketAsync(buyerId!);
    basket.GuardNull();

    await _basketService.RemoveBasketAsync(basket!);

    return RedirectToAction("index", "home");

  }


  [Route("baskettest")]
  public async Task<IActionResult> Test()
  {
    return Ok();
    //var basketService= Helper.GetBasketService(HttpContext);
    //var basket = await _basketService.GetOrCreateBasketAsync(Helper.GetUserId(User)!);
    //return Ok(basket?.BasketCreatedAt.ToString());

  }


}//eo class
