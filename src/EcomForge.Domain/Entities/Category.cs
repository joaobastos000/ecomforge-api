using EcomForge.Domain.Common;

namespace EcomForge.Domain.Entities;

public sealed class Category : Entity
{
    private Category() { }

    public Category(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
