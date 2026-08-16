using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.CustomerService.Dto;

public record CustomerResponse(int Id, string Name, CustomerTier Tier);