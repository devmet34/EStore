using Estore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Repos;
public class GenericReadRepo<TEntity> :IRepoRead<TEntity> where TEntity : class
{
  private readonly EStoreDbContext _dbContext;

  public GenericReadRepo(EStoreDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  IQueryable<TEntity> IRepoRead<TEntity>.Query => _dbContext.Set<TEntity>().AsNoTracking();

  public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
  }

  public async Task<IEnumerable<TEntity>?> ListByQueryAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
  {
    return await query.ToListAsync();
  }

}
