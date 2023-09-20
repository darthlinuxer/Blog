namespace Infra;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly BlogContext _context;
    public UnitOfWork(BlogContext context)
    {
        _context = context;
        Articles = new ArticleRepository(_context);
        Posts = new PostRepository(_context);
        Comments = new CommentRepository(_context);
    }
    public IArticleRepository Articles { get; init; }
    public IPostRepository Posts { get; init; }
    public ICommentRepository Comments { get; init; }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();

}