using Blog.API.Data;
using Blog.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Blog.API.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly BlogDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(BlogDbContext dbContext)
    {
        _dbSet = dbContext.Set<T>();
        _dbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }


    //public async Task<IEnumerable<Product>> Search(string name)
    //{
    //    IQueryable<Product> query = _dbContext.Products;

    //    if (!string.IsNullOrEmpty(name))
    //    {
    //        query = query.Where(e => e.ProductName.Contains(name));
    //    }

    //    return await query.ToListAsync();
    //}

    public async Task<IEnumerable<Product>> GetProductByName(string productName)
    {
        return await _dbContext.Products.Where(q => q.ProductName.Contains(productName)).ToListAsync();
    }


}

