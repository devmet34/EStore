using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Interfaces;
public interface IRepoRead<TEntity> where TEntity : class
{
  /// <summary>
  /// mc, Get queryable dbcontext with asnotracking/readonly. 
  /// </summary>
  public IQueryable<TEntity> Query { get; }
  public Task<IEnumerable<TEntity>?> ListByQueryAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);
  public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
