using EcomForge.Application.DTOs.Orders;
using EcomForge.Application.Services;
using EcomForge.Modules.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomForge.Modules.Orders.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public sealed class OrdersController(OrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await orderService.CreateAsync(request, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<IReadOnlyCollection<OrderDto>>> GetByCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        return Ok(await orderService.GetByCustomerAsync(customerId, cancellationToken));
    }
}
