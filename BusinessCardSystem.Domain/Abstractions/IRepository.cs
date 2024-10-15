namespace BizCardSystem.Domain.Abstractions;

public interface IRepository<T> where T : Entity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);

}