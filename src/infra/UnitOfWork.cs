namespace Infra;

public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private readonly BlogContext _context;
    public UnitOfWork(BlogContext context)
    {
        _context = context;
        Posts = new PostRepository(_context);
    }
    public IPostRepository Posts { get; private set; }
    public int Complete()
    {
        return _context.SaveChanges();
    }
    public void Dispose() => _context.Dispose();

}