namespace Infra.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.BaseUserId).IsRequired();
        builder.Property(c => c.PostId).IsRequired();

        builder.HasOne(c => c.Post)
             .WithMany(p => p.Comments)
             .HasForeignKey(c => c.PostId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.BaseUser)
               .WithMany(c => c.Comments)
               .HasForeignKey(c => c.BaseUserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ParentComment)
               .WithMany(c => c.Comments)
               .HasForeignKey(c => c.ParentCommentId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}