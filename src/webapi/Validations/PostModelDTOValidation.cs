namespace WebApi.Validations;

public class PostModelDTOValidation : AbstractValidator<PostModelDTO>, IValidator
{
    public PostModelDTOValidation(IServiceProvider serviceProvider)
    {
        RuleFor(_ => _.Title).NotEmpty();
        RuleFor(_ => _.Content).NotEmpty();
        RuleFor(_ => _.AuthorId).NotEmpty();
        RuleFor(_ => _.AuthorId).MustAsync(async (c, ct) =>
        {
            using var scope = serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var user = await userService.GetUserByIdAsync(c);
            return user.IsSuccess;
        });
    }
}