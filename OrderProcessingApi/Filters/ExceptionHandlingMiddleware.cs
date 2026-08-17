using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OrderProcessingApi.Filters;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, ProblemDetailsFactory problemDetailsFactory)
    {
        _next = next;
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred while processing the request.");

            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the exception handling middleware will not execute.");
                throw;
            }

            var (statusCode, problemDetails) = MapExceptionToProblemDetails(ex, context);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
        }
    }

    private (int, ProblemDetails) MapExceptionToProblemDetails(Exception ex, HttpContext context)
    {
        switch (ex)
        {
            case OrderProcessing.Application.Services.OrderService.NotFoundException notFound:
            {
                var pd = _problemDetailsFactory.CreateProblemDetails(context, statusCode: StatusCodes.Status404NotFound, title: "Not Found", detail: notFound.Message);
                pd.Type = "https://httpstatuses.com/404";
                return (StatusCodes.Status404NotFound, pd);
            }
            case OrderProcessing.Application.Services.OrderService.ConflictException conflict:
            {
                var pd = _problemDetailsFactory.CreateProblemDetails(context, statusCode: StatusCodes.Status409Conflict, title: "Conflict", detail: conflict.Message);
                pd.Type = "https://httpstatuses.com/409";
                return (StatusCodes.Status409Conflict, pd);
            }
            case OrderProcessing.Application.Services.OrderService.BadRequestException badRequest:
            {
                var pd = _problemDetailsFactory.CreateProblemDetails(context, statusCode: StatusCodes.Status400BadRequest, title: "Bad Request", detail: badRequest.Message);
                pd.Type = "https://httpstatuses.com/400";
                return (StatusCodes.Status400BadRequest, pd);
            }
            case FluentValidation.ValidationException validationException:
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                var pd = new ValidationProblemDetails(errors)
                {
                    Type = "https://httpstatuses.com/400",
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest
                };

                return (StatusCodes.Status400BadRequest, pd);
            }
            default:
            {
                var pd = _problemDetailsFactory.CreateProblemDetails(context, statusCode: StatusCodes.Status500InternalServerError, title: "An unexpected error occurred.");
                pd.Type = "https://httpstatuses.com/500";
                return (StatusCodes.Status500InternalServerError, pd);
            }
        }
    }
}
