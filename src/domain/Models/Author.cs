namespace Domain.Models;

//Authors are Special Kind of BaseUser with the Role Writer
//Authors have Posts
public class Author : BaseUser
{
    public ICollection<PostModel>? Posts { get; set; }

    public Author(string username) : base(username)
    {
        this.Role = Role.Writer;
    }

}