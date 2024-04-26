using Estore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Interfaces;
public interface IProductService
{

  public Task UpdateProduct(int productId, Product newProd);
  public Task DeleteProduct(int productId);

}
