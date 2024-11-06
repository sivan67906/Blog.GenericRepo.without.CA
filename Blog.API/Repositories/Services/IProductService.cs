using Blog.API.Data;
using Blog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Repositories.Services;

public interface IProductService : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductByName(string productName);
}
public class ProductService : Repository<Product>, IProductService
{
    public ProductService(BlogDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<IEnumerable<Product>> GetProductByName(string productName)
    {
        return await _dbSet.Where(q => q.ProductName.Contains(productName)).ToListAsync();
    }
}
