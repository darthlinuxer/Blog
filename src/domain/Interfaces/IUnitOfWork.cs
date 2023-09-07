namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IPostRepository Posts { get; init; }
    ICommentRepository Comments {get; init; }
    IUserRepository Users { get; init; }
    Task<int> CompleteAsync();
    void Dispose();
}