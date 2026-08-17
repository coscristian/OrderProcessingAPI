using System;

namespace OrderProcessing.Application.Services.OrderService;

public class ConflictException : Exception
{
    public ConflictException(string? message = null) : base(message)
    {
    }
}
