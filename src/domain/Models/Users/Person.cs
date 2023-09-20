namespace Domain.Models;

//All Users can post comments
public class Person : IdentityUser
{
    public Role Role { get; init; } = Role.Public;

    //Navigation Properties
    public virtual ICollection<Comment>? Comments { get; set; }
}