using Estore.Core.Entities;
using Estore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Services;
public class ProductService
{
  private readonly IRepo<Product> _repo;
  private readonly ILogger<ProductService> _logger;
  private readonly int pageSize = 10;

  public ProductService(IRepo<Product> repo, ILogger<ProductService> logger)
  {
    _repo = repo;
    _logger = logger;
  }

  public async Task<IEnumerable<Product>?> GetProductsAsync(int index=0, int page=1,string sortBy="Id")
  {
    var query = _repo.Query();
    sortBy = sortBy.ToLower(); 

    switch (sortBy) 
    { 
      case "id": query = query.OrderBy(p => p.Id);
        break;
      case "price": query=query.OrderBy(p => p.Price);
        break;
      case "category": query.OrderBy(p => p.Category);
        break;
      default: query = query.OrderBy(p => p.Id);
        break;

    }
      
    
    if (page > 1)
    {
      if (index > 0)
        query = query.Where(p => p.Id == index).Take(pageSize);
      else
      {
        query = query.Take(pageSize);
      } 

    }
    else
    {
      query=query.Take(pageSize);
    }


    var products= await _repo.ListByQuery(query);

    return products;
    //return await _repo.GetAllAsync();
    
  }

  

  public async Task<Product?> GetProductAsync(int productId)
  {
    return await _repo.GetByIdAsync(productId);
  }
}
