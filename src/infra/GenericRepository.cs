namespace Infra;

public class GenericPolimorphicRepository<T> : IGenericPolymorphicRepository<T> where T : class
{
    private readonly BlogContext _context;
    private readonly DbSet<T> _collection;
    public GenericPolimorphicRepository(BlogContext context)
    {
        _context = context;
        _collection = _context.Set<T>();
    }

    public bool Exist<U>(Expression<Func<U, bool>> p) where U : T => _collection.Any(p);

    public int Count<U>(Expression<Func<U, bool>> p) where U : T => _collection.Where(p).Count();

    public async Task AddAsync<U>(U entity) where U : T
    {
        try
        {
            await _collection.AddAsync(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Remove<U>(U entity) where U : T
    {
        try
        {
            _collection.Remove(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void RemoveRange<U>(U[] entities) where U : T
    {
        try
        {
            _collection.RemoveRange(entities.Cast<T>());
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Update<U>(U entity) where U : T
    {
        try
        {
            _collection.Update(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<U?> GetAsync<U>(Expression<Func<U, bool>> p,
                                      CancellationToken ct,
                                      bool asNoTracking,
                                      string[]? includeNavigationNames) where U : T
    {
        try
        {
            var mainQuery = _collection.AsQueryable<T>();
            if (asNoTracking) mainQuery = mainQuery.AsNoTracking();
            if (includeNavigationNames?.Length > 0) foreach (var navigation in includeNavigationNames) mainQuery = mainQuery.Include(navigation);
            return await mainQuery.OfType<U>().SingleOrDefaultAsync(p, ct);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public ConfiguredCancelableAsyncEnumerable<U>? GetAllAsync<U>(string where,
                                                                  string orderby,
                                                                  int page,
                                                                  int count,
                                                                  bool descending,
                                                                  string[]? includeNavigationNames,
                                                                  bool asNoTracking,
                                                                  CancellationToken ct) where U : T
    {
        try
        {
            string direction = descending ? "desc" : "asc";
            var mainQuery = _collection.Where(where).OrderBy($"{orderby} {direction}").Skip((page - 1) * count).Take(count);
            if (asNoTracking) mainQuery = mainQuery?.AsNoTracking();
            if (includeNavigationNames?.Length > 0) foreach (var navigation in includeNavigationNames) mainQuery = mainQuery!.Include(navigation);
            return mainQuery!.OfType<U>().AsAsyncEnumerable().WithCancellation(ct);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public ConfiguredCancelableAsyncEnumerable<U>? GetAllAsync<U>(
        Expression<Func<U, bool>> where,
        Expression<Func<U, string>> orderby,
        int page,
        int count,
        bool descending,
        string[]? includeNavigationNames,
        bool asNoTracking,
        CancellationToken ct) where U : T
    {
        try
        {
            string direction = descending ? "desc" : "asc";
            var mainQuery = _collection.Where(where).Skip((page - 1) * count).Take(count);
            if (asNoTracking) mainQuery = mainQuery?.AsNoTracking();
            if (includeNavigationNames?.Length > 0) foreach (var navigation in includeNavigationNames) mainQuery = mainQuery!.Include(navigation);
            var typed = mainQuery!.OfType<U>().OrderBy(orderby);
            return typed.AsAsyncEnumerable().WithCancellation(ct);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

    public async Task<T?> GetAsync(Expression<Func<T, bool>> p, CancellationToken ct, bool asNoTracking, string[]? includeNavigationNames)
    {
        try
        {
            var mainQuery = _collection.AsQueryable();
            if (asNoTracking) mainQuery = mainQuery.AsNoTracking();
            if (includeNavigationNames?.Length > 0) foreach (var navigation in includeNavigationNames) mainQuery = mainQuery.Include(navigation);
            return await mainQuery.SingleOrDefaultAsync(p, ct);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public ConfiguredCancelableAsyncEnumerable<T>? GetAllAsync(string where,
                                                               string orderby,
                                                               int page,
                                                               int count,
                                                               bool descending,
                                                               string[]? includeNavigationNames,
                                                               bool asNoTracking,
                                                               CancellationToken ct)
    {
        try
        {
            string direction = descending ? "desc" : "asc";
            var mainQuery = _collection.Where(where).OrderBy(where).Skip((page - 1) * count).Take(count);
            if (asNoTracking) mainQuery = mainQuery?.AsNoTracking();
            if (includeNavigationNames?.Length > 0) foreach (var navigation in includeNavigationNames) mainQuery = mainQuery!.Include(navigation);
            return mainQuery!.AsAsyncEnumerable<T>().WithCancellation(ct);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
