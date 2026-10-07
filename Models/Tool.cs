namespace ToolTracker.Models;

public sealed class Tool : Entity
{
    public string Name { get; }
    public string Category { get; }
    public Guid OwnerId { get; }

    public Tool(Guid id, string name, string category, Guid ownerId) : base(id)
    {
        Name = name;
        Category = category;
        OwnerId = ownerId;
    }
    public Tool(string name, string category, Guid ownerId) : this(Guid.NewGuid(), name, category, ownerId) { }
}