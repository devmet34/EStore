using Estore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Estore.Core.Interfaces
{
  public interface IRepo<T> where T : class
  {
    public IQueryable<T> Query();
    public IEnumerable<Product> GetProducts();
    public Task<T?> GetByQuery(IQueryable<T> query, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>?> ListByQueryAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    public  Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    public  Task<T?> GetBySpecAsync(ISpec<T> spec, CancellationToken cancellationToken = default);
    public  Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellationToken = default);
    public  Task<IEnumerable<T>?> ListBySpecAsync(CancellationToken cancellationToken = default);
    public  Task AddAsync(T entity, CancellationToken cancellationToken = default);
    public  Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    public  Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    public  Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(BaseEntity entity, CancellationToken cancellationToken = default);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default);

    //public Task AddWithSpecAsync();
  }
}
