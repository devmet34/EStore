using Estore.App.WebModels;
using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Models;
using EStore.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Estore.App.Services;
public class ProductService
{
  private readonly IRepoRead<Product> _readRepo;
  private readonly ILogger<ProductService> _logger;
  private readonly int pageSize;
  private IQueryable<Product>? query;
  private readonly IConfiguration _config;

  public ProductService(IRepoRead<Product> repo, ILogger<ProductService> logger, IConfiguration config)
  {
    _readRepo = repo;
    _logger = logger;
    _config = config;
    int.TryParse(config["DefaultPageSize"], out pageSize);
  }

  public async Task<IEnumerable<ProductVM>?> GetProductsPagedAsync(string? sortBy)
  {

    query = _readRepo.Query;
    sortBy = sortBy?.ToLower();

    //query=query.OrderBy(p => p.Name).Take(pageSize);
    SetSortForQuery(sortBy);

    var productVm = await query.Take(pageSize).Select(Projections.ProductToProductVM()).ToListAsync();

    //Helper.LogObjectHash(query);

    //var products = await query.AsNoTracking().Take(20).ToListAsync();
    //var products = await _repo.ListByQueryAsync(query);
    //var products = _repo.GetProducts();

    return productVm;
    //return await _repo.GetAllAsync();

  }
 

  public async Task<IEnumerable<ProductVM>?> GetProductsOnPageAsync(int page, string sortBy, string? find = null, FilterModel? filterModel = null)
  {
    page.GuardZero();
    page.GuardNegative();

    if (filterModel != null)
      query = SetFilterQuery(filterModel);
    else
      query = _readRepo.Query;

    SetSortForQuery(sortBy);
    

    if (find == null)
    {

      return await query.Skip((page - 1) * pageSize)
     .Take(pageSize).Select(Projections.ProductToProductVM()).ToListAsync();
      //return await _readRepo.ListByQueryAsync(query);
    }

    return await query.Where(p => p.Name.Contains(find))
    .Skip((page - 1) * pageSize).Take(pageSize).Select(Projections.ProductToProductVM()).ToListAsync();
    //return await _readRepo.ListByQueryAsync(query);

  }


  public async Task<IEnumerable<ProductVM>?> FindProductsAsync(string productName)
  {
    productName.GuardNullOrEmpty();
    return await _readRepo.Query
    .Where(p => p.Name.Contains(productName))
    .Take(pageSize).Select(Projections.ProductToProductVM()).ToListAsync();
    
  }


  public async Task<Product?> GetProductAsync(int productId)
  {
    return await _readRepo.GetByIdAsync(productId);
  }

  public async Task<Product?> GetProductForBasketAsync(int productId)
  {
    query = _readRepo.Query;

    Product pp = new Product("sdsd", 0, 0, 5, 5);
    return await _readRepo.Query.Where(p => p.Id == productId)
      .Select(p => new Product(p.Name, 0, 0, p.Price, p.Qt, null, p.PictureUri)).FirstOrDefaultAsync();
    //return await _repo.GetByIdAsync(productId);
  }

  public async Task<decimal> GetProductPriceAsync(int productId)
  {

    return await _readRepo.Query.Where(p => p.Id == productId).Select(p => p.Price).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<ProductVM>?> FilterProductsAsync(FilterModel filterModel)
  {
    query = SetFilterQuery(filterModel);
    /*
    int minPrice=filterModel.MinPrice;
    int maxPrice=filterModel.MaxPrice;

    if (minPrice > 0 || maxPrice > 0)
    {
      if (minPrice > 0 & maxPrice > 0)
        query = query.Where(p => p.Price > minPrice & p.Price < maxPrice);
      else if (minPrice > 0)
        query = query.Where(p => p.Price > minPrice);
      else if (maxPrice > 0)
        query = query.Where(p => p.Price < maxPrice);
    }

    if (!string.IsNullOrEmpty(filterModel.MainCat))
      query=query.Where(p => p.Category.MainCat == filterModel.MainCat);
    else if(!string.IsNullOrEmpty(filterModel.SubCat))
      query=query.Where(p => p.Category.SubCat == filterModel.SubCat);
    */

    return await query.Include(p => p.Category).Take(pageSize).Select(Projections.ProductToProductVM()).ToListAsync();

  }

  private IQueryable<Product> SetFilterQuery(FilterModel filterModel)
  {
    query = _readRepo.Query;
    int minPrice = filterModel.PriceMin;
    int maxPrice = filterModel.PriceMax;

    if (minPrice > 0 || maxPrice > 0)
    {
      if (minPrice > 0 & maxPrice > 0)
        query = query.Where(p => p.Price >= minPrice & p.Price <= maxPrice);
      else if (minPrice > 0)
        query = query.Where(p => p.Price >= minPrice);
      else if (maxPrice > 0)
        query = query.Where(p => p.Price <= maxPrice);
    }

    if (!string.IsNullOrEmpty(filterModel.MainCat))
      query = query.Where(p => p.Category!.MainCat == filterModel.MainCat);
    else if (!string.IsNullOrEmpty(filterModel.SubCat))
      query = query.Where(p => p.Category!.SubCat == filterModel.SubCat);
    return query;
  }

  private void SetSortForQuery(string? sortBy)
  {
    switch (sortBy)
    {
      case "name":
        query = query!.OrderBy(p => p.Name);
        break;
      case "price":
        query = query!.OrderBy(p => p.Price).ThenBy(p => p.Name);
        break;
      case "price_desc":
        query = query!.OrderByDescending(p => p.Price).ThenBy(p => p.Name);
        break;
      case "category":
        query = query!.OrderBy(p => p.Category).ThenBy(p => p.Name);
        break;
      default:
        query = query!.OrderBy(p => p.SortOrder);
        break;
    }

  }


}//eo class


