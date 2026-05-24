using EcomForge.Domain.Common;

namespace EcomForge.Domain.Entities;

public sealed class OrderItem : Entity
{
    private OrderItem() { }

    public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public Guid OrderId { get; private set; }
    public Order? Order { get; private set; }
}
