namespace Infra.Configurations;

public class EditorConfiguration : IEntityTypeConfiguration<Editor>
{
    public void Configure(EntityTypeBuilder<Editor> builder)
    {
        builder.HasMany(u => u.Comments)
             .WithOne(p => p.Person as Editor)
             .HasForeignKey(p => p.PersonId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}