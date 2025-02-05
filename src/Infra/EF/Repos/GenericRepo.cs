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

namespace EStore.Infra.EF.Repos;
public class GenericRepo<TEntity> : IRepo<TEntity> where TEntity : class, IAggregateRoot
{
    private readonly EStoreDbContext _dbContext;

    public GenericRepo(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;

    }

    //mc test debug
    public EStoreDbContext Context => _dbContext;

    public DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

    public IQueryable<TEntity> EvaluateSpec(IQueryable<TEntity> query, ISpec<TEntity> spec)
    {

        return SpecEvaluator.SetQuery(query, spec);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }
    public async Task<TEntity?> GetBySpecAsync(ISpec<TEntity> spec, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        query = EvaluateSpec(query, spec);
        return await query.FirstOrDefaultAsync();
    }
    /// <summary>
    /// mc return dbContext.Set<T> as queryable 
    /// </summary>
    /// <returns></returns>
    public IQueryable<TEntity> Query() { return _dbContext.Set<TEntity>().AsQueryable(); }

    public IEnumerable<Product> GetProducts()
    {
        throw new NotImplementedException();
        //return _dbContext.Products.OrderBy(p=>p.Name).Take(20).AsAsyncEnumerable();
    }

    public async Task<TEntity?> GetByQuery(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
    {
        /*
        var q = _dbContext.Set<T>().AsQueryable();
        q.Provider.CreateQuery(query.Expression);
        */

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>?> ListByQueryAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
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

        var q = _dbContext.Set<TEntity>().AsQueryable();



        //query=spec.Includes.Aggregate()


    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task<IEnumerable<TEntity>?> ListBySpecAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Add(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    Task<int> IRepoBase<TEntity>.SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
