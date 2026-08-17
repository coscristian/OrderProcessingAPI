using System;

namespace OrderProcessing.Application.Services.OrderService;

public class NotFoundException : Exception
{
    public NotFoundException(string? message = null) : base(message)
    {
    }
}
