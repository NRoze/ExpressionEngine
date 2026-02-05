using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpressionEngine.Infrastructure.Repositores;

public class EfRepository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _db;

    public EfRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _db.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _db.Set<T>().FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _db.Set<T>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;

        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;

        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync();
    }
}
