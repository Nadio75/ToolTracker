namespace ToolTracker.Models;

public sealed class Member : Entity
{
    public string Name { get; }
    public string Email { get; }

    public Member(Guid id, string name, string email) : base(id)
    {
        Name = name;
        Email = email;
    }
    public Member(string name, string email) : this(Guid.NewGuid(), name, email) { }
}