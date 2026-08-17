using System;

namespace OrderProcessing.Application.Services.OrderService;

public class BadRequestException : Exception
{
    public BadRequestException(string? message = null) : base(message)
    {
    }
}
