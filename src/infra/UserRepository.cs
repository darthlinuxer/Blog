namespace Infra;

public class UserRepository : GenericRepository<BaseUser>
{
    public UserRepository(BlogContext context) : base(context)
    {
    }

}