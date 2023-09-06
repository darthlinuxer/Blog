namespace Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IPostRepository Posts { get; }
    int Complete();
}