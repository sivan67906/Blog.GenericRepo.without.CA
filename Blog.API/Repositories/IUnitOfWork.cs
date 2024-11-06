using Blog.API.Repositories.Services;

namespace Blog.API.Repositories;
public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    public IProductService ProductService { get; }
    TRepository GetRepository<TRepository, TEntity>() where TRepository : class, IRepository<TEntity> where TEntity : class;
}