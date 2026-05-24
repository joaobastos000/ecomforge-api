using EcomForge.Domain.Common;
using EcomForge.Domain.Enums;

namespace EcomForge.Domain.Entities;

public sealed class Order : Entity
{
    private readonly List<OrderItem> _items = [];

    private Order() { }

    public Order(Guid customerId, IEnumerable<OrderItem> items)
    {
        CustomerId = customerId;
        _items.AddRange(items);
        Total = _items.Sum(item => item.UnitPrice * item.Quantity);
    }

    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal Total { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
}
