using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Services.CustomerService.Dto;
using OrderProcessing.Application.Services.CustomerService.Interfaces;

namespace OrderProcessingApi.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerResponse>> Create(
        CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.CreateAsync(
            request,
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, customer);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(
            id,
            cancellationToken);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }
}