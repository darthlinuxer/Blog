namespace Domain.Models;

public sealed record Comment : Article
{
    public int PostId { get; set; } //a comment is always linked to a PostId
    public string BaseUserId { get; set; } //everyone can write comments

    //Navigation Properties
    public PostModel? Post { get; set; }
    public Person? BaseUser { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public int? ParentCommentId {get; set;}
    public Comment? ParentComment {get; set;}

}