namespace Domain.Interfaces;

public interface IService<T> where T : class
{
    Task Add(object entity);
    Task Update(object key, object entity);
    Task Remove(object key);

}