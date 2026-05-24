namespace EcomForge.Application.DTOs.Orders;

public sealed record CreateOrderRequest(Guid CustomerId, IReadOnlyCollection<CreateOrderItemRequest> Items);
public sealed record CreateOrderItemRequest(Guid ProductId, int Quantity);
public sealed record OrderDto(Guid Id, Guid CustomerId, decimal Total, string Status, DateTime CreatedAtUtc, IReadOnlyCollection<OrderItemDto> Items);
public sealed record OrderItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);
