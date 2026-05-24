using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.Cart;
using EcomForge.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace EcomForge.Application.Services;

public sealed class CartService(IAppDbContext dbContext, ICartStore cartStore)
{
    public Task<CartDto> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return cartStore.GetAsync(customerId, cancellationToken);
    }

    public async Task<Result<CartDto>> AddItemAsync(Guid customerId, AddCartItemRequest request, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result<CartDto>.Failure(new Error("Products.NotFound", "Product not found."));
        }

        var cart = await cartStore.GetAsync(customerId, cancellationToken);
        var existing = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (existing is null)
        {
            cart.Items.Add(new CartItemDto(product.Id, product.Name, request.Quantity, product.Price));
        }
        else
        {
            cart.Items.Remove(existing);
            cart.Items.Add(existing with { Quantity = existing.Quantity + request.Quantity });
        }

        await cartStore.SaveAsync(cart, cancellationToken);
        return Result<CartDto>.Success(cart);
    }

    public Task ClearAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return cartStore.DeleteAsync(customerId, cancellationToken);
    }
}
