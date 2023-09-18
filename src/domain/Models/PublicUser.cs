namespace Domain.Models;

//PublicUsers are Special Kind of BaseUser with the Role Public
public class PublicUser : BaseUser
{
    public PublicUser(string username): base(username)
    {
        this.Role = Role.Public;   
    }

}