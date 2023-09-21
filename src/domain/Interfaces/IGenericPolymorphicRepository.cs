namespace Domain.Interfaces;

//Polimorphic generic Repository
public interface IGenericPolymorphicRepository<T> where T : class
{
    public bool Exist<U>(Expression<Func<U, bool>> p) where U : T;
    public int Count<U>(Expression<Func<U, bool>> p) where U : T;
    public Task<U?> AddAsync<U>(U entity) where U : T;
    public U Remove<U>(U entity) where U : T;
    public U Update<U>(U entity) where U : T;
    public Task<U?> GetAsync<U>(Expression<Func<U, bool>> p, CancellationToken ct, bool asNoTracking, string[]? includeNavigationNames) where U : T;
    public ConfiguredCancelableAsyncEnumerable<U>? GetAllAsync<U>(string where, string orderby, int page, int count, bool descending, string[]? includeNavigationNames, bool asNoTracking, CancellationToken ct) where U : T;
    public Task<int> CommitAsync();
}
