namespace Infra;

public class UserRepository : GenericRepository<BlogUser>
{
    public UserRepository(BlogContext context) : base(context)
    {
    }

}