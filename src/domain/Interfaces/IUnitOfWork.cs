namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IArticleRepository Articles { get; init; }
    IPostRepository Posts { get; init; }
    ICommentRepository Comments {get; init; }
    Task<int> CompleteAsync();
    void Dispose();
}