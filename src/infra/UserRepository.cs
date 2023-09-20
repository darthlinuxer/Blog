namespace Infra;

public class UserRepository : GenericRepository<Person>
{
    public UserRepository(BlogContext context) : base(context)
    {
    }

}