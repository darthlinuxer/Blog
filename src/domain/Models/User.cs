using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class User: IdentityUser
{
    public UserRole Role { get; set; }
    public ICollection<Post> Posts { get; set; }
}