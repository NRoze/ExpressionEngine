namespace ExpressionEngine.Core.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Query();
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task DeleteAsync(int id);
    }
}
