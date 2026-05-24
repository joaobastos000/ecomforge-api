using EcomForge.Application.DTOs.AI;
using EcomForge.Common.Results;

namespace EcomForge.Application.Abstractions;

public interface IAiChatService
{
    Task<Result<AiChatResponse>> ChatAsync(AiChatRequest request, CancellationToken cancellationToken = default);
}
