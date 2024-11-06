using Blog.API.Data;
using Blog.API.Repositories.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Entity;

namespace Blog.API.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly BlogDbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories;

    public IProductService ProductService { get; }

    private IDbContextTransaction _transaction;
    private bool _disposed = false;

    public UnitOfWork(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
        _repositories = new Dictionary<Type, object>();
        ProductService = new ProductService(dbContext);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        if (_repositories.ContainsKey(typeof(T)))
        {
            return _repositories[typeof(T)] as IRepository<T>;
        }

        var repo = new Repository<T>(_dbContext);
        _repositories.Add(typeof(T), repo);
        return repo;
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null!;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    TRepository IUnitOfWork.GetRepository<TRepository, TEntity>()
    {
        throw new NotImplementedException();
    }
}

