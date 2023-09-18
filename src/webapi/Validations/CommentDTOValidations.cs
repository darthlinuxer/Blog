namespace WebApi.Validations;

public class CommentDTOValidations : AbstractValidator<CommentDTO>
{
    public CommentDTOValidations()
    {
        RuleFor(_ => _.Title).NotEmpty();
        RuleFor(_ => _.Content).NotEmpty();
        RuleFor(_ => _.PostId).NotEmpty();
    }
}