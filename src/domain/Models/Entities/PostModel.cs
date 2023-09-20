namespace Domain.Models;

public sealed record PostModel: Article
{
    public string AuthorId { get; set; }	
	public Author Author {get; set;}
	public ICollection<Comment>? Comments {get; set;}

}