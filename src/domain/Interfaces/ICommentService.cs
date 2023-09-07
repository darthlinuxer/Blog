namespace Domain.Interfaces;

 public interface ICommentService: IGenericRepository<Comment>,  ICommentRepository
{
    public Task<int> CompleteAsync();
}