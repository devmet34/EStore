using AutoMapper;
using AutoMapper.QueryableExtensions;
using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using EStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace EStore.App.Services;
public class BasketDBService : IBasketDBService
{
  private readonly IRepo<Basket> _repo;
  private readonly ILogger<BasketDBService> _logger;
  private readonly ProductService _productService;
  private readonly IMapper _mapper;

  public BasketDBService(IRepo<Basket> repo, ILogger<BasketDBService> logger, ProductService productService, IMapper mapper)
  {
    _repo = repo;
    _logger = logger;
    _productService = productService;
    _mapper = mapper;

  }


  public async Task CreateBasketAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();

    //var basketSpec=new BasketSpec(buyerId);
    var hasBasketCreated = await _repo.Query.AsNoTracking().Where(b => b.BuyerId == buyerId).AnyAsync();
    if (hasBasketCreated)
    {
      _logger.LogDebug("Basket already exist on DB");
      return;
    }

    _logger.LogDebug("Creating basket on DB");
    Basket basket = new(buyerId);
    await _repo.AddAsync(basket);
  }

  /// <summary>
  /// mc, Get basket and basketitems only.
  /// </summary>
  /// <param name="buyerId"></param>
  /// <returns></returns>
  public async Task<Basket?> GetBasketAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();
    return await _repo.Query.Where(b => b.BuyerId == buyerId)
    .Include(b => b.BasketItems).FirstOrDefaultAsync();

  }

  public async Task<BasketVM?> GetBasketVMAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();
    var query = _repo.Query.AsNoTracking();
    return await query.Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems).ThenInclude(i => i.Product).ProjectTo<BasketVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

  }


  public async Task<int> GetBasketCountAsync(string buyerId)
  {
    var count = await _repo.Query.AsNoTracking().Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems).Select(b => b.BasketItems.Count).FirstOrDefaultAsync();
    return count;
  }


  public async Task SetBasketItemAsync(string buyerId, int productId, int qt)
  {
    //var basketSpec = new BasketSpec(buyerId);
    var basket = await GetBasketAsync(buyerId);
    basket.GuardNull();

    var productPrice = await _productService.GetProductPriceAsync(productId);
    basket!.SetBasketItem(productId, qt, productPrice);
    await _repo.UpdateAsync(basket);

  }


  public async Task RemoveBasketItemAsync(string buyerId, int productId)
  {
    _logger.LogDebug("cus_log: Removing basket item for userId: " + buyerId);
    var basket = await GetBasketAsync(buyerId);
    basket.GuardNull();
    //var productPrice=await _productService.GetProductPriceAsync(productId);
    basket!.RemoveBasketItem(productId);

    await _repo.UpdateAsync(basket);

  }

  public async Task RemoveBasketAsync(string buyerId)
  {
    await _repo.Query.Where(b => b.BuyerId == buyerId).ExecuteDeleteAsync();
  }



}
