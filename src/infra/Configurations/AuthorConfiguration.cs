namespace Infra.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasMany(u => u.Posts)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Comments)
             .WithOne(p => p.Person as Author)
             .HasForeignKey(p => p.PersonId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}