namespace Domain.Models;

public class PostModel
{
    public int? PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; set; }
    public Status PostStatus { get; set; } = Status.draft;
    public string? AuthorId { get; set; }
    public BaseUser? Author { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}