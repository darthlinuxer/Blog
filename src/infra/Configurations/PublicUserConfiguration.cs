namespace Infra.Configurations;

public class PublicUserConfiguration : IEntityTypeConfiguration<PublicUser>
{
    public void Configure(EntityTypeBuilder<PublicUser> builder)
    {
        builder.HasMany(u => u.Comments)
               .WithOne(p => p.Person as PublicUser)
               .HasForeignKey(p => p.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

    }

}