namespace Domain.Models;

//Article is the base class for every Post and Comment in the Post
//Every Post and Comment is an Article
public abstract class Article
{
    public int? Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; init; }
    public Status Status { get; set; } = Status.draft;

    public Article(string title, string content, DateTime? datePublished = null)
    {
        this.Title = title;
        this.Content = content;
        this.DatePublished = datePublished ?? DateTime.Now;
    }
}