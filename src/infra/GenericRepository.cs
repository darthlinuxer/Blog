namespace Infra;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly BlogContext _context;
    public GenericRepository(BlogContext context)
    {
        _context = context;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> p, CancellationToken ct, bool asNoTracking, string[]? includeNavigationNames)
    {
        try
        {
            var pred = p.Compile();
            var mainQuery = _context.Set<T>().Where(p);
            if (asNoTracking) mainQuery = mainQuery.AsNoTracking();
            if (includeNavigationNames?.Length > 0) foreach (var navigation in includeNavigationNames) mainQuery = mainQuery.Include(navigation);
            var result = await mainQuery.SingleOrDefaultAsync(ct);
            return result;
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
            var mainQuery = _context.Set<T>().Where(where).OrderBy($"{orderby} {direction}").Skip((page - 1) * count).Take(count);
            if (asNoTracking) mainQuery = mainQuery?.AsNoTracking();
            if (includeNavigationNames?.Length > 0) foreach (var navigation in includeNavigationNames) mainQuery = mainQuery!.Include(navigation);
            var result = mainQuery?.AsAsyncEnumerable().WithCancellation(ct);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<T?> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }


    public T Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }

    public T Update(T entity)
    {
        var entry = _context.Set<T>().Update(entity);
        entry.State = EntityState.Modified;
        return entity;
    }

}
