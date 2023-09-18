
namespace Domain.Models;

//A Comment is an Article associated with a Post.
//Comments can have other Comments
public class Comment: Article
{
    public Comment(int postId, string baseUserId, string title, string content, DateTime? datePublished = null) : base(title, content, datePublished)
    {
        this.PostId = postId;
        this.BaseUserId = baseUserId;
    }

    public int PostId { get; set; }
    public string BaseUserId {get; set;}

    //Navigation Properties
    public BaseUser? BaseUser {get; set;}
    public PostModel? Post { get; set; }
    public ICollection<Comment>? Comments {get; set;}

}