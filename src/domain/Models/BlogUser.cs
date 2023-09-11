namespace Domain.Models;

public class BlogUser : IdentityUser
{
    public ICollection<PostModel> Posts { get; set; }
}