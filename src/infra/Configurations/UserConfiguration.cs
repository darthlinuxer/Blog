namespace Infra.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Define the primary key
        builder.HasKey(u => u.UserId);

        // Configure other properties with column names
        builder.Property(u => u.UserId).ValueGeneratedOnAdd();
        builder.Property(u => u.Username).HasMaxLength(255).IsRequired();
        builder.Property(u => u.Password).HasMaxLength(255).IsRequired();
        builder.Property(u => u.Role).IsRequired();

        builder.HasMany(u => u.Posts)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.PostId)
               .OnDelete(DeleteBehavior.Cascade);
        
    }
}