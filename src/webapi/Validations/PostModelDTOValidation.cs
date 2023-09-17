namespace WebApi.Validations;

public class PostModelDTOValidations : AbstractValidator<PostModelDTO>
{
    public PostModelDTOValidations()
    {
        RuleFor(_ => _.Title).NotEmpty();
        RuleFor(_ => _.Content).NotEmpty();
    }
}