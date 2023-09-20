namespace Domain.Models;

public sealed record CommentDTO : ArticleDTO
{
    [Required]
    public int PostId { get; set; } //a comment is always linked to a PostId
    [Required]
    public string BaseUserId { get; set; } //everyone can write comments

    //Navigation Properties
    //init because you can not transfer a comment to another Post
    public PostModel? Post { get; init; }

    //init because you can not transfer a comment to another user
    public Person? BaseUser { get; init; }

    public ICollection<CommentDTO>? Comments { get; set; }

    public CommentDTO(string baseUserId,
                      string title,
                      string content,
                      int postId,
                      DateTime? datePublished = null) : base(title, content, datePublished)
    {
        PostId = postId;
        BaseUserId = baseUserId;
    }

    public static implicit operator Comment(CommentDTO dto)
    {
        // var comment = new Comment(
        //     title: dto.Title,
        //     content: dto.Content,
        //     postId: dto.PostId,
        //     personId: dto.BaseUserId
        // )
        // {
        //     Post = dto.Post,
        //     Comments = dto.Comments?.Select(c => (Comment)c).ToList()
        // };
        var comment = new Comment()
        {
            Title = dto.Title,
            Content = dto.Content,
            PostId = dto.PostId,
            PersonId = dto.BaseUserId,
            Post = dto.Post,
            Comments = dto.Comments?.Select(c => (Comment)c).ToList()
        };
        

        if (dto.Id is not null) comment.Id = dto.Id;
        comment.Status = dto.Status;

        return comment;
    }
}