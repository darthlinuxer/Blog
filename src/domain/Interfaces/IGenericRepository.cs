namespace Domain.Interfaces;
public interface IGenericRepository<T> where T : class
{
    public Task<T?> AddAsync(T entity);
    public ConfiguredCancelableAsyncEnumerable<T> GetAllAsync(string where, string orderby, int page, int count, bool descending, string[]? includeNavigationNames, bool asNoTracking, CancellationToken ct);
    public Task<T?> GetAsync(Expression<Func<T, bool>> p, CancellationToken ct, bool asNoTracking, string[]? includeNavigationNames);
    public T Remove(T entity);
    public T Update(T entity);
    public bool Exist(Expression<Func<T,bool>> p);
    public int Count(Expression<Func<T,bool>> p);
}
