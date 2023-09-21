namespace Domain.Models;

//PublicUsers are Special Kind of BaseUser with the Role Public
public sealed class Editor : Person
{
    public Editor()
    {
        Role = UserRole.Editor;
    }
}