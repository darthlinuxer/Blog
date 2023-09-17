using SQLitePCL;

namespace WebApi.Validations;

public class PostLifeCycleValidations : AbstractValidator<PostLifeCycle>
{
    public PostLifeCycleValidations(
    )
    {
        RuleFor(_ => _.Role)
        .Must(role => !string.IsNullOrEmpty(role)).WithMessage("Role cannot be null or empty")
        .Must(role=> new string[]{"Writer","Editor","Public"}.Contains(role));
        RuleFor(_=>_.AuthorIdRequestingChange).NotEmpty().WithMessage("Author cannot be empty");

        RuleFor(_=>_).Must(_=>_.PostId != null && _.PostId>0)
        .WithMessage("PostId cannot be null or < 0")       
        .Must(cycle =>
        {
            return cycle.IsValid();
        })
        .WithMessage(cycle => $"Operation Invalid for Role {cycle.Role} moving status from {cycle.Status} to {cycle.MoveToStatus}");
    }
}