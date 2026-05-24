using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.Orders;
using EcomForge.Common.Results;
using EcomForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomForge.Application.Services;

public sealed class OrderService(IAppDbContext dbContext)
{
    public async Task<Result<OrderDto>> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var productIds = request.Items.Select(x => x.ProductId).ToArray();
        var products = await dbContext.Products.Where(x => productIds.Contains(x.Id)).ToListAsync(cancellationToken);

        if (products.Count != productIds.Distinct().Count())
        {
            return Result<OrderDto>.Failure(new Error("Orders.InvalidProducts", "One or more products were not found."));
        }

        var items = new List<OrderItem>();
        foreach (var requestItem in request.Items)
        {
            var product = products.Single(x => x.Id == requestItem.ProductId);
            if (product.Stock < requestItem.Quantity)
            {
                return Result<OrderDto>.Failure(new Error("Orders.InsufficientStock", $"Insufficient stock for {product.Name}."));
            }

            product.DecreaseStock(requestItem.Quantity);
            items.Add(new OrderItem(product.Id, product.Name, requestItem.Quantity, product.Price));
        }

        var order = new Order(request.CustomerId, items);
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<OrderDto>.Success(Map(order));
    }

    public async Task<IReadOnlyCollection<OrderDto>> GetByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders.AsNoTracking()
            .Include(x => x.Items)
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => Map(x))
            .ToListAsync(cancellationToken);
    }

    private static OrderDto Map(Order order)
    {
        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Total,
            order.Status.ToString(),
            order.CreatedAtUtc,
            order.Items.Select(x => new OrderItemDto(x.ProductId, x.ProductName, x.Quantity, x.UnitPrice)).ToList());
    }
}
