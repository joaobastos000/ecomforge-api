using EcomForge.Domain.Common;

namespace EcomForge.Domain.Entities;

public sealed class Product : Entity
{
    private Product() { }

    public Product(string name, string description, decimal price, int stock, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public void Update(string name, string description, decimal price, int stock, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        if (Stock < quantity)
        {
            throw new InvalidOperationException("Insufficient product stock.");
        }

        Stock -= quantity;
    }
}
