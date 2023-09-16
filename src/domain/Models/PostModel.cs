namespace Domain.Models;

public class PostModel
{
    public int? PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; set; }
    public PostStatus PostStatus {get; set;} = PostStatus.draft;
    public string? AuthorId { get; set; }
    public BlogUser? Author { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}