using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.CustomerService.Dto;

public record CreateCustomerRequest(string Name, CustomerTier Tier);