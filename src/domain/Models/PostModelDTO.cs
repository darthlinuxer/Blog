namespace Domain.Models;

public class PostModelDTO
{
	public int? PostId { get; set; }
	public required string Title { get; set; }
	public required string Content { get; set; }
	public DateTime? DatePublished { get; set; }
	public required string AuthorId { get; set; }
	public BlogUser? Author { get; set; }
	public ICollection<Comment>? Comments { get; set; }

	public static implicit operator PostModel(PostModelDTO dto)
	{
		return new PostModel
		{
			Title = dto.Title,
			Content = dto.Content,
			DatePublished = (dto.DatePublished is null) ? DateTime.Now : dto.DatePublished.Value,
			AuthorId = dto.AuthorId,
			Comments = dto.Comments ?? new List<Comment>()
		};
	}
}