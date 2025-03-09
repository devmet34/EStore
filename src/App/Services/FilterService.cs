using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.App.Services;
public class FilterService
{
  private readonly ILogger<FilterService> _logger;
  private readonly IRepo<Category> _repo;
  private readonly ProductService _productService;

  public FilterService(ILogger<FilterService> logger, IRepo<Category> repo, ProductService productService)
  {
    _logger = logger;
    _repo = repo;
    _productService = productService;
  }

  public async Task<IEnumerable<Category>?> GetCatsAsync()
  {
    return await _repo.GetAllAsync();

  }

  public async Task<Dictionary<string, List<string>>?> GetCatsAsDictAsync()
  {
    var cats = await _repo.GetAllAsync();
    if (cats == null)
      return null;
    var dict = new Dictionary<string, List<string>>();

    foreach (var cat in cats)
    {
      string mainCat = cat.MainCat;
      var subCat = cat.SubCat;
      //dict already has the key 
      if (dict.ContainsKey(mainCat))
      {
        if (subCat == null)
          continue;
        dict[mainCat].Add(subCat);
        continue;
      }
      //dict has no given key so adding key/val pair
      if (subCat != null)
        dict.Add(mainCat, new List<string>() { subCat });
      else { dict.Add(mainCat, new List<string>()); }

    }
    return dict;

  }

  public async Task<IEnumerable<Product>?> FilterAsync(FilterModel filterModel)
  {
    filterModel.GuardNull();
    return await _productService.FilterProductsAsync(filterModel);

  }



}
