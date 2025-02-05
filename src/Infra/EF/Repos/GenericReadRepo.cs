using Estore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Repos;
public class GenericReadRepo:IRepoRead
{
  private readonly EStoreDbContext _dbContext;

  public GenericReadRepo(EStoreDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  /// <summary>
  /// mc, Get queryable dbcontext with asnotracking. 
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  /// <returns></returns>
  public IQueryable<TEntity> Query<TEntity>() where TEntity : class
  {
    return _dbContext.Set<TEntity>().AsNoTracking();
  }
}
