using Microsoft.EntityFrameworkCore;


namespace EStore.Core.Interfaces
{
  public interface IRepo<TEntity> : IRepoBase<TEntity> where TEntity : class
  {
    public IQueryable<TEntity> Query { get; }
    public DbSet<TEntity> DbSet { get; }

    public Task<TEntity?> GetByQuery(IQueryable<TEntity> query, CancellationToken cancellationToken = default);
    public Task<IEnumerable<TEntity>?> ListByQueryAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);
    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<TEntity?> GetBySpecAsync(ISpec<TEntity> spec, CancellationToken cancellationToken = default);

    public Task<IEnumerable<TEntity>?> ListBySpecAsync(CancellationToken cancellationToken = default);
    //public Task DeleteAsync(BaseEntity entity, CancellationToken cancellationToken = default);


    //public Task AddWithSpecAsync();
  }
}
