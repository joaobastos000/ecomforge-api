namespace EcomForge.Infrastructure.AI;

public sealed class AiOptions
{
    public string Provider { get; init; } = "OpenAI";
    public string Model { get; init; } = "gpt-4.1-mini";
    public string ApiKey { get; init; } = string.Empty;
}
