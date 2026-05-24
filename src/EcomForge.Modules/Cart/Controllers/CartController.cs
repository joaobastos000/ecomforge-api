using EcomForge.Application.DTOs.Cart;
using EcomForge.Application.Services;
using EcomForge.Modules.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomForge.Modules.Cart.Controllers;

[ApiController]
[Authorize]
[Route("api/cart")]
public sealed class CartController(CartService cartService) : ControllerBase
{
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CartDto>> Get(Guid customerId, CancellationToken cancellationToken)
    {
        return Ok(await cartService.GetAsync(customerId, cancellationToken));
    }

    [HttpPost("{customerId:guid}/items")]
    public async Task<ActionResult<CartDto>> AddItem(Guid customerId, AddCartItemRequest request, CancellationToken cancellationToken)
    {
        var result = await cartService.AddItemAsync(customerId, request, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpDelete("{customerId:guid}")]
    public async Task<ActionResult> Clear(Guid customerId, CancellationToken cancellationToken)
    {
        await cartService.ClearAsync(customerId, cancellationToken);
        return NoContent();
    }
}
