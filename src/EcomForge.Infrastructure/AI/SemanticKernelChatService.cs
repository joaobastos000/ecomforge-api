using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.AI;
using EcomForge.Common.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace EcomForge.Infrastructure.AI;

public sealed class SemanticKernelChatService(
    IOptions<AiOptions> options,
    RedisAiMemory memory,
    IEcommerceAiPlugin ecommercePlugin,
    ILogger<SemanticKernelChatService> logger) : IAiChatService
{
    private readonly AiOptions _options = options.Value;

    public async Task<Result<AiChatResponse>> ChatAsync(AiChatRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            return Result<AiChatResponse>.Success(new AiChatResponse(
                request.ConversationId,
                "AI is configured, but AI__ApiKey is empty. Add a key to enable Semantic Kernel chat."));
        }

        try
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(_options.Model, _options.ApiKey);
            var kernel = builder.Build();
            kernel.Plugins.AddFromObject(ecommercePlugin, "Ecommerce");

            var previousMemory = await memory.GetAsync(request.ConversationId);
            var prompt = $"""
                You are EcomForge Assistant, a concise e-commerce helper.
                Use the Ecommerce plugin when product recommendation or order status context is useful.

                Conversation memory:
                {previousMemory}

                User:
                {request.Message}
                """;

            var answer = (await kernel.InvokePromptAsync(prompt, cancellationToken: cancellationToken)).ToString();
            var updatedMemory = $"{previousMemory}\nUser: {request.Message}\nAssistant: {answer}".Trim();
            await memory.SaveAsync(request.ConversationId, updatedMemory[^Math.Min(updatedMemory.Length, 4000)..]);

            return Result<AiChatResponse>.Success(new AiChatResponse(request.ConversationId, answer));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Semantic Kernel chat failed.");
            return Result<AiChatResponse>.Failure(new Error("AI.ChatFailed", "AI chat failed."));
        }
    }
}
