namespace Domain.Models;

public sealed record PostModelDTO: ArticleDTO
{
    public PostModelDTO(string authorId, string title, string content, DateTime? datePublished = null) : base(title, content, datePublished)
    {
		AuthorId = authorId;
    }

	[Required]
    public string AuthorId { get; init; }

	public ICollection<CommentDTO>? Comments {get; set;}

	public static implicit operator PostModel(PostModelDTO dto)
	{
		// return new PostModel(dto.AuthorId, dto.Title, dto.Content)
		// {
		// 	Id = dto.Id,
		// 	Comments = dto.Comments?.Select(c=>(Comment)c).ToList()
		// };

		return new PostModel()
		{
			AuthorId = dto.AuthorId,
			Title = dto.Title,
			Content = dto.Content,
			Id = dto.Id,
			Comments = dto.Comments?.Select(c=>(Comment)c).ToList()
		};
	}
}