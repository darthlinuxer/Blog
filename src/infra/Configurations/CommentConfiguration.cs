namespace Infra.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.PersonId).IsRequired();
        builder.Property(c => c.PostId).IsRequired();

        builder.HasOne(c => c.Post)
             .WithMany(p => p.Comments)
             .HasForeignKey(c => c.PostId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Person)
               .WithMany(c => c.Comments)
               .HasForeignKey(c => c.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ParentComment)
               .WithMany(c => c.Comments)
               .HasForeignKey(c => c.ParentCommentId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}