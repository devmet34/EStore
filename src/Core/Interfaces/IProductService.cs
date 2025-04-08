using EStore.Core.Entities;

namespace EStore.Core.Interfaces;
public interface IProductService
{

  public Task UpdateProduct(int productId, Product newProd);
  public Task DeleteProduct(int productId);

}
