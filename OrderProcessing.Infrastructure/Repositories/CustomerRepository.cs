using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Services.CustomerService.Interfaces;
using OrderProcessing.Domain.Aggregates.CustomerAggregate;
using OrderProcessing.Infrastructure.Persistence;

namespace OrderProcessing.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly OrderProcessingDbContext _context;

    public CustomerRepository(OrderProcessingDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(
                customer => customer.Id == id,
                cancellationToken);
    }

    public async Task<Customer> AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(
            customer,
            cancellationToken);

        return customer;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}