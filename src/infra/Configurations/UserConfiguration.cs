namespace Infra.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<BlogUser>
{
    public void Configure(EntityTypeBuilder<BlogUser> builder)
    {

        builder.HasMany(u => u.Posts)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}