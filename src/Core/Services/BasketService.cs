using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Specs;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;







namespace Estore.Core.Services;
public class BasketService 
{
  private readonly IRepo<Basket> _repo;
  private readonly ILogger<BasketService> _logger;
  private readonly ProductService _productService;
  //private readonly Basket basket;
  public BasketService(IRepo<Basket> repo, ILogger<BasketService> logger,ProductService productService )
  {
    _repo = repo;
    _logger = logger;
    _productService = productService;
    
    //_logger = loggerFactory.CreateLogger<BasketService>();


  }

  public async Task<Basket?> GetOrCreateBasketAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();
    
    
    
    //var basketSpec=new BasketSpec(buyerId);
    var basket=await GetBasketAsync(buyerId);
    if (basket!=null) {
      _logger.LogDebug("basket already exist");
      return basket;
    }
    _logger.LogInformation("creating basket");
    basket = new(buyerId);
    await _repo.AddAsync(basket);
    return await GetBasketAsync(buyerId);
  }
  /*
  public async Task<Basket?> TestQuery()
  {


  }
  */

  public async Task<Basket?> GetBasketAsync(string buyerId, bool includeBasketItems=true, bool includeAll=false)
  {
    buyerId.GuardNullOrEmpty();
    var query=_repo.Query();
    query = query.Where(b => b.BuyerId == buyerId);



    if (includeAll)
    {
      query = query.Include(b => b.BasketItems).ThenInclude(i => i.Product);
    }

    else if (includeBasketItems)
      query = query.Include(b => b.BasketItems);


    return await _repo.GetByQuery(query);
  }

  public async Task SetBasketItemAsync(string buyerId, int productId,int qt)
  {
    //var basketSpec = new BasketSpec(buyerId);
    var basket = await GetBasketAsync(buyerId);
    basket.GuardNull();

    var product = await _productService.GetProductAsync(productId);
    product.GuardNull();

    basket!.SetBasketItem(productId, product!.Name, qt,product!.Price);
    await _repo.UpdateAsync(basket);


  }

  /*
  public async Task AddProductAsync( string buyerId,int productId)
  {
    var basketSpec = new BasketSpec(buyerId);
    var basket = await _repo.GetBySpecAsync(basketSpec);       
    basket.GuardNull();
    
    var product=await _productService.GetProductAsync(productId);
    product.GuardNull();

    basket!.AddItem(productId);
    await _repo.UpdateAsync(basket);
   

  }
  */

  public async Task RemoveBasketItemAsync(string buyerId, int productId)
  {
    _logger.LogDebug("cus_log: Removing basket item for userId: " + buyerId);
    var basket=await GetBasketAsync(buyerId,true);
    basket.GuardNull();

    basket!.RemoveBasketItem(productId);

    await _repo.UpdateAsync(basket);

    /*
    var basketItem = basket?.BasketItems.Where(bi => bi.ProductId == productId).FirstOrDefault();
    basketItem.GuardNull();
    */
    //await _repo.DeleteAsync(basketItem!);
    
  }

  public async Task RemoveBasketAsync(Basket basket)
  {
    await _repo.DeleteAsync(basket);
  }

  public async Task SubtractProductAsync(string buyerId, int productId)
  {
    var basketSpec = new BasketSpec(buyerId);
    var basket = await _repo.GetBySpecAsync(basketSpec);
    basket.GuardNull();

    var product = await _productService.GetProductAsync(productId);
    product.GuardNull();

    //basket!.SubtractItem(productId);
    await _repo.UpdateAsync(basket);

  }

  public void Test()
  {
    _logger.LogInformation("test from basketservice");
    var basket = new Basket("test");
  }
}
