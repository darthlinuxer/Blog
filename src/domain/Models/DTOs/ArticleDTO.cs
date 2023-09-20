namespace Domain.Models;

public abstract record ArticleDTO
{
    public int? Id { get; set; }

    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime DatePublished { get; init; }
    
    public Status Status { get; set; } = Status.draft;

    public ArticleDTO(string title, string content, DateTime? datePublished = null)
    {
        Title = title;
        Content = content;
        DatePublished = datePublished ?? DateTime.Now;
    }
}