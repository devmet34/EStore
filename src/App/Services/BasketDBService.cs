using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using EStore.Core.Specs;
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



namespace EStore.App.Services;
public class BasketDBService : IBasketDBService
{
  private readonly IRepo<Basket> _repo;  
  private readonly ILogger<BasketDBService> _logger;
  private readonly ProductService _productService;
  //private readonly Basket basket;
  public BasketDBService(IRepo<Basket> repo, ILogger<BasketDBService> logger, ProductService productService)
  {
    _repo = repo;
    _logger = logger;
    _productService = productService;

    //_logger = loggerFactory.CreateLogger<BasketService>();


  }

  //mc debug test
  public IRepo<Basket> Repo => _repo;

  public async Task CreateBasketAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();

    //var basketSpec=new BasketSpec(buyerId);
    var hasBasketCreated = await _repo.Query.AsNoTracking().Where(b=>b.BuyerId == buyerId).AnyAsync();
    if (hasBasketCreated)
    {
      _logger.LogDebug("Basket already exist on DB");
      return;
    }

    _logger.LogDebug("Creating basket on DB");
    Basket basket = new(buyerId);
    await _repo.AddAsync(basket);    
  }

  public async Task<Basket?> GetBasketAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();
    var query = _repo.Query;
    query = query.Where(b => b.BuyerId == buyerId);
    query = query.Include(b => b.BasketItems).ThenInclude(i => i.Product);

    return await _repo.GetByQuery(query);
  }

  [Obsolete]
  public async Task<Basket?> GetBasketAsync(string buyerId, bool includeBasketItems = true, bool includeAll = false)
  {
    buyerId.GuardNullOrEmpty();
    var query = _repo.Query;
    query = query.Where(b => b.BuyerId == buyerId);



    if (includeAll)
    {
      query = query.Include(b => b.BasketItems).ThenInclude(i => i.Product);
    }

    else if (includeBasketItems)
      query = query.Include(b => b.BasketItems);


    return await _repo.GetByQuery(query);
  }


  public async Task<int> GetBasketCountAsync(string buyerId)
  {     
    var count= await _repo.Query.AsNoTracking().Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems).Select(b => b.BasketItems.Count).FirstOrDefaultAsync();
    return count;
  }


  public async Task SetBasketItemAsync(string buyerId, int productId, int qt)
  {
    //var basketSpec = new BasketSpec(buyerId);
    var basket = await GetBasketAsync(buyerId);
    basket.GuardNull();

    var product = await _productService.GetProductForBasketAsync(productId);
    product.GuardNull();

    basket!.SetBasketItem(productId, qt, product!.Price);
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
    var basket = await GetBasketAsync(buyerId);
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
    await _repo.UpdateAsync(basket!);

  }

  public void Test()
  {
    _logger.LogInformation("test from basketservice");
    var basket = new Basket("test");
  }

  public Basket? GetBasket(string cacheKey)
  {
    throw new NotImplementedException();
  }

 
}
