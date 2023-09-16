namespace Infra.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<PostModel>
{
       public void Configure(EntityTypeBuilder<PostModel> builder)
       {
              // Define the primary key
              builder.HasKey(p => p.PostId);

              // Configure other properties with column names and constraints
              builder.Property(p => p.PostId)
                     .ValueGeneratedOnAdd();

              builder.Property(p => p.Title)
                     .HasMaxLength(255)
                     .IsRequired();

              builder.Property(p => p.Content)
                     .IsRequired();
              
               builder.Property(p => p.PostStatus)
                     .IsRequired();

              builder.Property(p => p.DatePublished)
                     .IsRequired();

              builder.Property(p => p.AuthorId)
                     .IsRequired();

              builder.HasMany(p => p.Comments)
                     .WithOne(p => p.Post)
                     .HasForeignKey(p => p.PostId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasOne(p => p.Author)
                     .WithMany(u => u.Posts)
                     .HasForeignKey(p => p.AuthorId)
                     .OnDelete(DeleteBehavior.Restrict);
       }
}