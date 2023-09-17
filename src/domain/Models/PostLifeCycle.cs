namespace Domain.Models;

public class PostLifeCycle
{

    public int? PostId { get; set; }
    public string? AuthorIdRequestingChange { get; set; }
    public string? Role { get; set; }
    public required PostStatus Status { get; set; }
    public required PostStatus MoveToStatus { get; set; }

    public bool IsValid()
    {
        //Writer Powers
        //All states can be moved to Draft again
        if (Role == "Writer" && Status == PostStatus.pending && MoveToStatus == PostStatus.draft) return true;
        if (Role == "Writer" && Status == PostStatus.approved && MoveToStatus == PostStatus.draft) return true;
        if (Role == "Writer" && Status == PostStatus.rejected && MoveToStatus == PostStatus.draft) return true;
        if (Role == "Writer" && Status == PostStatus.published && MoveToStatus == PostStatus.draft) return true;

        //Moves to Pending: Only a Draft can be moved to pending
        if (Role == "Writer" && Status == PostStatus.draft && MoveToStatus == PostStatus.pending) return true;

        //Moves to Published: Only aproved Posts can be moved to Published
        if (Role == "Writer" && Status == PostStatus.approved && MoveToStatus == PostStatus.published) return true;

        //-------------------------------------------------------------------------------
        //Editor Powers: a Pending Post can be moved to approved or rejected
        //Moves to Rejected
        if (Role == "Editor" && Status == PostStatus.pending && MoveToStatus == PostStatus.rejected) return true;
        if (Role == "Editor" && Status == PostStatus.approved && MoveToStatus == PostStatus.rejected) return true;
        if (Role == "Editor" && Status == PostStatus.published && MoveToStatus == PostStatus.rejected) return true;

        //Moves to Aproved
        if (Role == "Editor" && Status == PostStatus.pending && MoveToStatus == PostStatus.approved) return true;
        if (Role == "Editor" && Status == PostStatus.rejected && MoveToStatus == PostStatus.approved) return true;

        return false;
    }

}