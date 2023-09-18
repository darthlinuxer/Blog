namespace Domain.Models;

public class PostLifeCycle
{

    public int? PostId { get; set; }
    public string? AuthorIdRequestingChange { get; set; }
    public string? Role { get; set; }
    public required Status Status { get; set; }
    public required Status MoveToStatus { get; set; }

    public bool IsValid()
    {
        //Writer Powers
        //All states can be moved to Draft again
        if (Role == "Writer" && Status == Status.pending && MoveToStatus == Status.draft) return true;
        if (Role == "Writer" && Status == Status.approved && MoveToStatus == Status.draft) return true;
        if (Role == "Writer" && Status == Status.rejected && MoveToStatus == Status.draft) return true;
        if (Role == "Writer" && Status == Status.published && MoveToStatus == Status.draft) return true;

        //Moves to Pending: Only a Draft can be moved to pending
        if (Role == "Writer" && Status == Status.draft && MoveToStatus == Status.pending) return true;

        //Moves to Published: Only aproved Posts can be moved to Published
        if (Role == "Writer" && Status == Status.approved && MoveToStatus == Status.published) return true;

        //-------------------------------------------------------------------------------
        //Editor Powers: a Pending Post can be moved to approved or rejected
        //Moves to Rejected
        if (Role == "Editor" && Status == Status.pending && MoveToStatus == Status.rejected) return true;
        if (Role == "Editor" && Status == Status.approved && MoveToStatus == Status.rejected) return true;
        if (Role == "Editor" && Status == Status.published && MoveToStatus == Status.rejected) return true;

        //Moves to Aproved
        if (Role == "Editor" && Status == Status.pending && MoveToStatus == Status.approved) return true;
        if (Role == "Editor" && Status == Status.rejected && MoveToStatus == Status.approved) return true;

        return false;
    }

}