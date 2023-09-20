namespace Domain.Models;

public sealed record PostModelDTO: ArticleDTO
{
    public PostModelDTO(string authorId, string title, string content, DateTime? datePublished = null) : base(title, content, datePublished)
    {
		AuthorId = authorId;
    }

	[Required]
    public string AuthorId { get; init; }

	public ICollection<CommentDTO>? CommentDTOs {get; set;}

	public static implicit operator PostModel(PostModelDTO dto)
	{
		return new PostModel()
		{
			AuthorId = dto.AuthorId,
			Title = dto.Title,
			Content = dto.Content,
			Id = dto.Id,
		};
	}
}