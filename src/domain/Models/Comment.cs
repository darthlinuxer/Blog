namespace Domain.Models;

public class Comment
{
    public int CommentId { get; set; }
    public string Text { get; set; }
    DateTime DateTime { get; set; } = DateTime.Now;
    public int PostId { get; set; }
    public PostModel Post { get; set; }
}