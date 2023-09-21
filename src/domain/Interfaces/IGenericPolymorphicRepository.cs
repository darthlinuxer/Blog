namespace Domain.Interfaces;

//Polimorphic generic Repository
public interface IGenericPolymorphicRepository<T> where T : class
{
    bool Exist<U>(Expression<Func<U, bool>> p) where U : T;
    int Count<U>(Expression<Func<U, bool>> p) where U : T;
    Task AddAsync<U>(U entity) where U : T;
    void Remove<U>(U entity) where U : T;
    void RemoveRange<U>(U[] entities) where U : T;
    void Update<U>(U entity) where U : T;

    Task<T?> GetAsync(Expression<Func<T, bool>> p,
                      CancellationToken ct,
                      bool asNoTracking,
                      string[]? includeNavigationNames);
    Task<U?> GetAsync<U>(Expression<Func<U, bool>> p,
                         CancellationToken ct,
                         bool asNoTracking,
                         string[]? includeNavigationNames) where U : T;

    public ConfiguredCancelableAsyncEnumerable<T>? GetAllAsync(
        string where,
        string orderby,
        int page,
        int count,
        bool descending,
        string[]? includeNavigationNames,
        bool asNoTracking,
        CancellationToken ct);

    public ConfiguredCancelableAsyncEnumerable<U>? GetAllAsync<U>(
        string where,
        string orderby,
        int page,
        int count,
        bool descending,
        string[]? includeNavigationNames,
        bool asNoTracking,
        CancellationToken ct) where U : T;

    public ConfiguredCancelableAsyncEnumerable<U>? GetAllAsync<U>(
        Expression<Func<U, bool>> where,
        Expression<Func<U, string>> orderby,
        int page,
        int count,
        bool descending,
        string[]? includeNavigationNames,
        bool asNoTracking,
        CancellationToken ct) where U : T;

    Task<int> CommitAsync();
}
