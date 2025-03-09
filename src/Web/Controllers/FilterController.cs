using AutoMapper;
using Estore.App.Services;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Estore.Core.Models;
using EStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EStore.Web.Controllers;
public class FilterController : Controller
{
  private readonly ILogger<FilterController> _logger;
  private readonly FilterService _filterService;
  private readonly IMapper _mapper;


  public FilterController(ILogger<FilterController> logger, FilterService filterService, IMapper mapper)
  {
    _logger = logger;
    _filterService = filterService;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> GetCats()
  {
    var cats=await _filterService.GetCatsAsDictAsync();
    return Ok(cats);
  }

  [HttpGet]
  [Route("filterproducts")]
  public async Task<IActionResult> FilterProducts(FilterModel filterModel )
  {
    filterModel.GuardNull();
    if (!ModelState.IsValid)
      throw new ArgumentException();

    var products = await _filterService.FilterAsync(filterModel);
    //var productVM = _mapper.Map<IEnumerable<ProductVM>>(products);

    return PartialView("_productcards", products);
    
  }

}
