namespace Domain.Models;

//Authors are Special Kind of BaseUser with the Role Writer
//Authors have Posts
public class Author : Person
{
    //Navigation Property
    public virtual ICollection<PostModel>? Posts { get; set; }

    public Author()
    {
        Role = Role.Writer;
    }
}