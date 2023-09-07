namespace Infra;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly BlogContext _context;
    public UnitOfWork(BlogContext context)
    {
        _context = context;
        Posts = new PostRepository(_context);
        Comments = new CommentRepository(_context);
        Users = new UserRepository(_context);
    }
    public IPostRepository Posts { get; init; }
    public ICommentRepository Comments {get; init; }
    public IUserRepository Users { get; init; }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();

}