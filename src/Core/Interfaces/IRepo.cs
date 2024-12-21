using Estore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Estore.Core.Interfaces
{
  public interface IRepo<T>:IRepoBase<T> where T : class
  {
    public IQueryable<T> Query();
    public DbSet<T> DbSet { get; }
    public IEnumerable<Product> GetProducts();
    public Task<T?> GetByQuery(IQueryable<T> query, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>?> ListByQueryAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    public  Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    public  Task<T?> GetBySpecAsync(ISpec<T> spec, CancellationToken cancellationToken = default);
   
    public  Task<IEnumerable<T>?> ListBySpecAsync(CancellationToken cancellationToken = default);    
    public Task DeleteAsync(BaseEntity entity, CancellationToken cancellationToken = default);


    //public Task AddWithSpecAsync();
  }
}
