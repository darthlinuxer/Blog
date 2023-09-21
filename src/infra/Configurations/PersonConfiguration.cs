namespace Infra.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.Property(p => p.Role).IsRequired();

        builder.HasMany(u => u.Comments)
             .WithOne(p => p.BaseUser)
             .HasForeignKey(p => p.BaseUserId)
             .OnDelete(DeleteBehavior.Cascade);

    }
}