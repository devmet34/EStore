using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Models;
public class FilterModel
{
  
  public string? MainCat { get; set; }
  public string? SubCat { get; set; }
  public int PriceMin { get; set; }
  public int PriceMax { get; set; }
}
