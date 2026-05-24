namespace EcomForge.Application.DTOs.Cart;

public sealed record CartDto(Guid CustomerId, List<CartItemDto> Items)
{
    public decimal Total => Items.Sum(item => item.UnitPrice * item.Quantity);
}

public sealed record CartItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);
public sealed record AddCartItemRequest(Guid ProductId, int Quantity);
