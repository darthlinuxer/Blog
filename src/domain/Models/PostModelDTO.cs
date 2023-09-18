namespace Domain.Models;

public class PostModelDTO
{
	public int? PostId { get; set; }
	public required string Title { get; set; }
	public required string Content { get; set; }
	public DateTime? DatePublished { get; set; }
	public string? AuthorId { get; set; }

	[JsonIgnore]
	public Status PostStatus { get; set; } = Status.draft;

	public static implicit operator PostModel(PostModelDTO dto)
	{
		return new PostModel
		{
			PostId = dto.PostId,
			Title = dto.Title,
			Content = dto.Content,
			DatePublished = (dto.DatePublished is null) ?
							DateTime.Now :
							dto.DatePublished.Value,
			AuthorId = dto.AuthorId,
			PostStatus = dto.PostStatus
		};
	}
}