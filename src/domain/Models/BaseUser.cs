namespace Domain.Models;

//All Users can post comments
public abstract class BaseUser : IdentityUser
{
    public Role Role {get; init;}
    public ICollection<Comment>? Comments {get; set;}

    public BaseUser(string username)
    {
        this.UserName = username;
    }

}