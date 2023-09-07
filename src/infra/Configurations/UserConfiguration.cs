namespace Infra.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<BlogUser>
{
    public void Configure(EntityTypeBuilder<BlogUser> builder)
    {
        // // Define the primary key
        // builder.HasKey(u => u.Id);

        // // Configure other properties with column names
        // builder.Property(u => u.Id).ValueGeneratedOnAdd();
        // builder.Property(u => u.Role).IsRequired();

        builder.HasMany(u => u.Posts)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}