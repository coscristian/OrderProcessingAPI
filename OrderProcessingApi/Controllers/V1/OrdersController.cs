using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Services.OrderService;
using OrderProcessing.Application.Services.OrderService.Dto;

namespace OrderProcessingApi.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var response = await _orderService.CreateAsync(request, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<PagedOrdersResponse>> GetPaged(int page = 1, int pageSize = 10, int? customerId = null, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        var result = await _orderService.GetPagedAsync(page, pageSize, customerId, from, to, cancellationToken);

        return Ok(result);
    }
}