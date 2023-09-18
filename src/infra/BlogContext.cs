namespace Infra;
public class BlogContext : IdentityDbContext<BaseUser>
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {
    }

    public DbSet<PostModel> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}