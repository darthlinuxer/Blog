namespace Infra.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<PostModel>
{
       public void Configure(EntityTypeBuilder<PostModel> builder)
       {
          
              builder.Property(p => p.AuthorId).IsRequired();

              builder.HasMany(p => p.Comments)
                     .WithOne(p => p.Post)
                     .HasForeignKey(p => p.PostId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasOne(p => p.Author)
                     .WithMany(u => u.Posts)
                     .HasForeignKey(p => p.AuthorId)
                     .OnDelete(DeleteBehavior.Cascade);

       }
}