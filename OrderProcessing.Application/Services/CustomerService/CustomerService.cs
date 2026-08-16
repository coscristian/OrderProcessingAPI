using OrderProcessing.Application.Services.CustomerService.Dto;
using OrderProcessing.Application.Services.CustomerService.Interfaces;
using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.CustomerService;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(
        ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var customer = Customer.Create(
            request.Name,
            request.Tier);

        await _customerRepository.AddAsync(
            customer,
            cancellationToken);

        await _customerRepository.SaveChangesAsync(
            cancellationToken);

        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Tier);
    }

    public async Task<CustomerResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (customer is null)
            return null;

        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Tier);
    }
}