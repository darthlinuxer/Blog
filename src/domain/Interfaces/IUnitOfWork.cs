namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IGenericPolymorphicRepository<Article> Articles { get; init; }
    IGenericPolymorphicRepository<Person> Persons { get; init; }
    Task<int> CompleteAsync();
    void Dispose();
}