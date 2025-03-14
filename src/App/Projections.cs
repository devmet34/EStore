using EStore.App.WebModels;
using EStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EStore.App;
public class Projections
{
  public static Expression<Func<Product, ProductVM>> ProductToProductVM()
  {
    return product => new ProductVM() 
    { Id = product.Id, Name = product.Name, PictureUri = product.PictureUri, Price = product.Price, Qt = product.Qt };

  }
}
