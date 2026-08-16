using OrderProcessing.Application.Services.CustomerService.Dto;

namespace OrderProcessing.Application.Services.CustomerService.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}