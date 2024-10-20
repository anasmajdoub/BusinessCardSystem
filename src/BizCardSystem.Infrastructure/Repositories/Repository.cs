using BizCardSystem.Domain.Abstractions;
using BizCardSystem.Infrastructure.DataBaseConext;
using Microsoft.EntityFrameworkCore;

namespace BizCardSystem.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly ApplicationDbContext _context;

    private DbSet<T> _Table;
    public IQueryable<T> QueryAsNoTracking => Query.AsNoTracking();
    public IQueryable<T> Query => _Table.AsQueryable();

    public Repository(ApplicationDbContext context)
    {
        this._context = context;
        _Table = context.Set<T>();
    }

    public async Task CreateAsync(T entity)
    {
        await _Table.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            _Table.Remove(_Table.Find(id)!);
            await SaveAsync();
        }
        catch (Exception ex)
        {

        }

    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        try
        {
            return await QueryAsNoTracking.ToListAsync();
        }
        catch (Exception e)
        {
            //look at this
        }
        return default!;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        try
        {
            return (await _Table.FindAsync(id))!;
        }
        catch (Exception e)
        {
            //look at this 
        }
        return default!;
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _Table.Update(entity);
            await SaveAsync();
        }
        catch (Exception e)
        {

        }

    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
