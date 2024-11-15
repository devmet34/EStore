using Estore.Core.Entities;
using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Interfaces;
using EStore.Infra.EF.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EStore.Infra.EF;
public class EfRepo<T>:IRepo<T> where T : class, IAggregateRoot
{
  private readonly EStoreDbContext _dbContext;

  public EfRepo(EStoreDbContext dbContext) 
  { 
    _dbContext = dbContext;

  }

  public EStoreDbContext DbContext => _dbContext;

  public IQueryable<T> EvaluateSpec(IQueryable<T> query, ISpec<T> spec)
  {
    return SpecEvaluator.SetQuery(query, spec);
  }

  public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
  }
  public async Task<T?> GetBySpecAsync(ISpec<T> spec, CancellationToken cancellationToken = default)
  {
    var query = _dbContext.Set<T>().AsQueryable();
    
    query = EvaluateSpec(query, spec);
    return await query.FirstOrDefaultAsync();
  }

  public IQueryable<T> Query() { return _dbContext.Set<T>().AsQueryable(); }

  public IEnumerable<Product> GetProducts() {
    throw new NotImplementedException();
    //return _dbContext.Products.OrderBy(p=>p.Name).Take(20).AsAsyncEnumerable();
  }  

  public async Task<T?> GetByQuery(IQueryable<T> query, CancellationToken cancellationToken = default)
  {
    /*
    var q = _dbContext.Set<T>().AsQueryable();
    q.Provider.CreateQuery(query.Expression);
    */
    
    return await query.FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<T>?> ListByQueryAsync(IQueryable<T> query,CancellationToken cancellationToken=default)
  {
    return await query.ToListAsync();
  }

    public async Task DeleteAsync(BaseEntity entity, CancellationToken cancellationToken)
  {
    _dbContext.Remove(entity);
    await _dbContext.SaveChangesAsync();
  }

  public async Task Test()
  {

    var query = _dbContext.Set<Basket>().AsQueryable();
    query.Include(x => x.BasketItems).ThenInclude(y => y.Product);

    var q= _dbContext.Set<T>().AsQueryable();
    


    //query=spec.Includes.Aggregate()


  }

  public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
 

  public async Task<IEnumerable<T>?> ListBySpecAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<T>?> GetAllNoTrackingAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

  }
  public async Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    
  }

  public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    _dbContext.Set<T>().Add(entity);
    await SaveChangesAsync(cancellationToken);
  }

  public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    _dbContext.Set<T>().Update(entity);
    await SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    _dbContext.Remove(entity);
    await SaveChangesAsync(cancellationToken);
  }


  
}
