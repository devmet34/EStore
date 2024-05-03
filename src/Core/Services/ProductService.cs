using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
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
  private readonly int pageSize = 20;
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

    
          

    Helper.LogObjectHash(query);

    //var products = await query.AsNoTracking().Take(20).ToListAsync();
    var products= await _repo.ListByQueryAsync(query);
    //var products = _repo.GetProducts();
    

    return products;
    //return await _repo.GetAllAsync();
    
  }

  public async Task<IEnumerable<Product>?> GetProductsOnPageAsync(int page, string sortBy, string? find = null)
  {
    page.GuardZero();
    page.GuardNegative();

    query = _repo.Query();
        

    if ( find == null)
    {
      SetSort(sortBy);
      query = query.Skip((page-1)*pageSize);
      query = query.Take(pageSize);      
      return await _repo.ListByQueryAsync(query);
    }

   

    throw new NotImplementedException();
   
  }

  

  public async Task<Product?> GetProductAsync(int productId)
  {
    return await _repo.GetByIdAsync(productId);
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
        query.OrderBy(p => p.Category).ThenBy(p => p.Name);
        break;
      default:
        query = query.OrderBy(p => p.Id);
        break;
    }
    
  }
}
