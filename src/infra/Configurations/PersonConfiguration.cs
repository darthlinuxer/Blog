namespace Infra.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.Role).IsRequired();

        builder.HasDiscriminator<string>("PersonType")
                  .HasValue<Author>("Author")
                  .HasValue<Editor>("Editor")
                  .HasValue<PublicUser>("PublicUser");


        builder.HasMany(u => u.Comments)
             .WithOne(p => p.BaseUser)
             .HasForeignKey(p => p.BaseUserId)
             .OnDelete(DeleteBehavior.Cascade);

    }
}