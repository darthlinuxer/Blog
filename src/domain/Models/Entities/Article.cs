namespace Domain.Models;

public abstract record Article
{
    public int? Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; init; }
    public Status Status { get; set; } = Status.draft;

}