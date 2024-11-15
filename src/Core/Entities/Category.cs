using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;
public class Category:BaseEntity,IAggregateRoot
{
  public string MainCat { get;private set; }
  public string? SubCat {  get;private set; }
  public string? Description { get; private set; }

  public Category(string mainCat, string? subCat, string? description)
  {
    MainCat = mainCat;
    SubCat=subCat;
    Description = description;
  }
}
