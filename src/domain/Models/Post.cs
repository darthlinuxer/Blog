namespace Domain.Models;

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; set; }
    public User Author { get; set; }
    public ICollection<Comment> Comments { get; set; }
}