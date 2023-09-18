namespace Infra.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Define the primary key
        builder.HasKey(c => c.CommentId);

        // Configure other properties with column names and constraints
        builder.Property(c => c.CommentId).ValueGeneratedOnAdd();
        builder.Property(c => c.Content)
            .IsRequired();

        builder.Property(c => c.PostId)
            .IsRequired();

        builder.HasOne(c => c.Post)
             .WithMany(p => p.Comments)
             .HasForeignKey(c => c.PostId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}