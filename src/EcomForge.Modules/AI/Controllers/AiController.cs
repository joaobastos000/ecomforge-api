using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.AI;
using EcomForge.Modules.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomForge.Modules.AI.Controllers;

[ApiController]
[Authorize]
[Route("api/ai")]
public sealed class AiController(IAiChatService chatService) : ControllerBase
{
    [HttpPost("chat")]
    public async Task<ActionResult<AiChatResponse>> Chat(AiChatRequest request, CancellationToken cancellationToken)
    {
        var result = await chatService.ChatAsync(request, cancellationToken);
        return this.ToActionResult(result);
    }
}
