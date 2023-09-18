namespace Domain.Models;

public class CommentDTO
{
    public int? CommentId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; set; }
    public int PostId { get; set; }
    public PostModel? Post { get; set; }
    public string BaseUserId {get; set;}

    [JsonIgnore]
    public Status? CommentStatus { get; set; }

    public CommentDTO(string userId, string title, string content, int postId, DateTime? datePublished = null)
    {
        Title = title;
        Content = content;
        PostId = postId;
        DatePublished = datePublished ?? DateTime.Now;
        BaseUserId = userId;
    }

    public bool IsValid([FromServices] IValidator<CommentDTO> commentValidator)
    {
        var result = commentValidator.Validate(this);
        return result.IsValid;
    }

    public static implicit operator Comment(CommentDTO dto)
    {
        var comment = new Comment(
            title: dto.Title,
            content: dto.Content,
            postId: dto.PostId,
            datePublished: dto.DatePublished,
            baseUserId: dto.BaseUserId
        );

        if (dto.CommentId is not null) comment.Id = dto.CommentId;
        if (dto.CommentStatus is not null) comment.Status = dto.CommentStatus.Value;

        return comment;
    }
}