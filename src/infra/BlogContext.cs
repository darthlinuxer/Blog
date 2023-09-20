namespace Infra;
public class BlogContext : IdentityDbContext<IdentityUser>
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<PostModel> Posts { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<PublicUser> PublicUsers { get; set; }
    public DbSet<Editor> Editors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new EditorConfiguration());
        modelBuilder.ApplyConfiguration(new PublicUserConfiguration());

        modelBuilder.Entity<Person>().UseTptMappingStrategy();
        modelBuilder.Entity<Article>().UseTptMappingStrategy();

        base.OnModelCreating(modelBuilder);
    }
}