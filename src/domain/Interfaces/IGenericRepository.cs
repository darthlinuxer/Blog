namespace Domain.Interfaces;
public interface IGenericRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T> FindAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    IQueryable<T> Find(Expression<Func<T, bool>> expression);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
}
