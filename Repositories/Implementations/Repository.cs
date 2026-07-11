using LMS.API.Data;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LMSDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(LMSDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

}
