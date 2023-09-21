namespace Domain.Models;

//All Users can post comments
public class Person : IdentityUser
{
    public UserRole Role { get; init; } = UserRole.Public;

    //Navigation Properties
    public virtual ICollection<Comment>? Comments { get; set; }
}