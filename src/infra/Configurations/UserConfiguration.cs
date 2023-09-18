namespace Infra.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<BaseUser>
{
    public void Configure(EntityTypeBuilder<BaseUser> builder)
    {

        builder.HasMany(u => u.Posts)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}