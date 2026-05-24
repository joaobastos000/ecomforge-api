using EcomForge.Application.DTOs.Cart;

namespace EcomForge.Application.Abstractions;

public interface ICartStore
{
    Task<CartDto> GetAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task SaveAsync(CartDto cart, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid customerId, CancellationToken cancellationToken = default);
}
