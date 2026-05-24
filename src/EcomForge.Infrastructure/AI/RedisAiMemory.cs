using EcomForge.Common.Constants;
using StackExchange.Redis;

namespace EcomForge.Infrastructure.AI;

public sealed class RedisAiMemory(IConnectionMultiplexer redis)
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<string> GetAsync(string conversationId)
    {
        var value = await _database.StringGetAsync(GetKey(conversationId));
        return value.HasValue ? value! : string.Empty;
    }

    public Task SaveAsync(string conversationId, string memory)
    {
        return _database.StringSetAsync(GetKey(conversationId), memory, TimeSpan.FromHours(12));
    }

    private static string GetKey(string conversationId) => $"{AppConstants.AiMemoryKeyPrefix}{conversationId}";
}
