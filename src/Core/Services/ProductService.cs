using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Models;
using EStore.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Services;
public class ProductService
{
  private readonly IRepo<Product> _repo;
  private readonly ILogger<ProductService> _logger;
  private readonly int pageSize = Constants.pageSize;
  private IQueryable<Product>? query;

  public ProductService(IRepo<Product> repo, ILogger<ProductService> logger)
  {
    _repo = repo;
    _logger = logger;
  }

  public async Task<IEnumerable<Product>?> GetProductsAsync( string sortBy="name")
  {
    
    query = _repo.Query();
    sortBy = sortBy.ToLower();

    //query=query.OrderBy(p => p.Name).Take(pageSize);
    SetSort(sortBy);

    query =query.AsNoTracking().Take(pageSize);
    var q=query.Include(p=>p.Category).AsNoTracking().Take(pageSize);
    var l = await q.ToListAsync();      
          

    //Helper.LogObjectHash(query);

    //var products = await query.AsNoTracking().Take(20).ToListAsync();
    var products= await _repo.ListByQueryAsync(query);
    //var products = _repo.GetProducts();
    

    return products;
    //return await _repo.GetAllAsync();
    
  }

  public async Task<IEnumerable<Product>?> GetProductsOnPageAsync(int page, string sortBy, string? find = null,FilterModel? filterModel=null)
  {
    page.GuardZero();
    page.GuardNegative();

    if (filterModel != null)
      query=SetFilterQuery(filterModel);
    else
      query = _repo.Query();


    SetSort(sortBy);


    if ( find == null)
    {
      
      query = query.Skip((page-1)*pageSize);
      query = query.AsNoTracking().Take(pageSize);      
      return await _repo.ListByQueryAsync(query);
    }

    
    query = query.Where(p => p.Name.Contains(find));
    query = query.Skip((page - 1) * pageSize);
    query = query.AsNoTracking().Take(pageSize);
    return await _repo.ListByQueryAsync(query);



    
   
  }


  public async Task<IEnumerable<Product>?> FindProductsAsync(string productName)
  {
    productName.GuardNullOrEmpty();
    query = _repo.Query();
    query = query.Where(p => p.Name.Contains(productName));
    query=  query.AsNoTracking().Take(pageSize);
    return await _repo.ListByQueryAsync(query);
  }
  

  public async Task<Product?> GetProductAsync(int productId)
  {
    return await _repo.GetByIdAsync(productId);
  }

  public async Task<decimal> GetProductPriceAsync(int productId)
  {
    
    return await _repo.Query().Where(p => p.Id == productId).Select(p => p.Price).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Product>?> FilterProductsAsync(FilterModel filterModel)
  {
    query= SetFilterQuery(filterModel);
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

    return await query.Include(p=>p.Category).AsNoTracking().Take(pageSize).ToListAsync();

  }

  private IQueryable<Product> SetFilterQuery(FilterModel filterModel) {
    query=_repo.Query();
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
      query = query.Where(p => p.Category.MainCat == filterModel.MainCat);
    else if (!string.IsNullOrEmpty(filterModel.SubCat))
      query = query.Where(p => p.Category.SubCat == filterModel.SubCat);
    return query;
  }

  private void SetSort(string sortBy)
  {
    switch (sortBy)
    {
      case "name":
        query = query.OrderBy(p => p.Name);
        break;
      case "price":
        query = query.OrderBy(p => p.Price).ThenBy(p=>p.Name);
        break;
      case "price_desc":
        query = query.OrderByDescending(p => p.Price).ThenBy(p => p.Name);
        break;
      case "category":
        query=query.OrderBy(p => p.Category).ThenBy(p => p.Name);
        break;
      default:
        query = query.OrderBy(p => p.Id);
        break;
    }
    
  }

  public async Task UpdateProductsBatch()
  {
     
  }

}//eo class


