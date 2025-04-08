using EStore.Core.Entities;
using EStore.Core.Models;
using System.Linq.Expressions;

namespace EStore.App;
public class Projections
{
  public static Expression<Func<Product, ProductVM>> ProductToProductVM()
  {
    return product => new ProductVM()
    { Id = product.Id, Name = product.Name, PictureUri = product.PictureUri, Price = product.Price };

  }
}
