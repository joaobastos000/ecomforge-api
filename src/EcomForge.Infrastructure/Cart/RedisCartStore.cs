using System.Text.Json;
using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.Cart;
using EcomForge.Common.Constants;
using StackExchange.Redis;

namespace EcomForge.Infrastructure.Cart;

public sealed class RedisCartStore(IConnectionMultiplexer redis) : ICartStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<CartDto> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var value = await _database.StringGetAsync(GetKey(customerId));
        return value.HasValue
            ? JsonSerializer.Deserialize<CartDto>(value!, JsonOptions) ?? new CartDto(customerId, [])
            : new CartDto(customerId, []);
    }

    public Task SaveAsync(CartDto cart, CancellationToken cancellationToken = default)
    {
        return _database.StringSetAsync(GetKey(cart.CustomerId), JsonSerializer.Serialize(cart, JsonOptions), TimeSpan.FromDays(7));
    }

    public Task DeleteAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return _database.KeyDeleteAsync(GetKey(customerId));
    }

    private static string GetKey(Guid customerId) => $"{AppConstants.CartKeyPrefix}{customerId}";
}
