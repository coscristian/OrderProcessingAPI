using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.CustomerService.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Customer> AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}