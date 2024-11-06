using Blog.API.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace Blog.API.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    //Task<IEnumerable<Product>> Search(string name);

    Task<IEnumerable<Product>> GetProductByName(string productName);

}

