namespace WebApi.Validations;

public class PostModelDTOValidation : AbstractValidator<PostModelDTO>
{
    public PostModelDTOValidation()
    {
        RuleFor(_ => _.Title).NotEmpty();
        RuleFor(_ => _.Content).NotEmpty();
    }
}