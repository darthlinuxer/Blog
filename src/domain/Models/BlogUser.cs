using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class BlogUser : IdentityUser
{
    public ICollection<Post> Posts { get; set; }
}