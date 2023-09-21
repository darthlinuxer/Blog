namespace Infra.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
       public void Configure(EntityTypeBuilder<Article> builder)
       {
              builder.UseTptMappingStrategy();

              // Define the primary key
              builder.HasKey(p => p.Id);
              builder.Property(p => p.Id).ValueGeneratedOnAdd();
              builder.Property(p => p.Title).HasMaxLength(255).IsRequired();
              builder.Property(p => p.Content).IsRequired();
              builder.Property(p => p.Status).IsRequired();
              builder.Property(p => p.DatePublished).IsRequired();
       }
}