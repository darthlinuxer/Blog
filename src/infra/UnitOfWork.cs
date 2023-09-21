namespace Infra;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly BlogContext _context;
    public UnitOfWork(BlogContext context)
    {
        _context = context;
        Articles = new GenericRepository<Article>(_context);
        Persons = new GenericRepository<Person>(_context);
        
    }
    public IGenericPolymorphicRepository<Article> Articles { get; init; }
    public IGenericPolymorphicRepository<Person> Persons {get; init;}
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();

}