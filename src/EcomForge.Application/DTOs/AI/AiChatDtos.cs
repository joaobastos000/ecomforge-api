namespace EcomForge.Application.DTOs.AI;

public sealed record AiChatRequest(string ConversationId, string Message);
public sealed record AiChatResponse(string ConversationId, string Answer);
