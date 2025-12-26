using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using GreenConnectPlatform.Business.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Api.Configurations;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var statusCode = StatusCodes.Status500InternalServerError;
        var errorCode = "INTERNAL_SERVER_ERROR";
        var message = "An unexpected server error has occurred.";

        switch (exception)
        {
            case ApiExceptionModel apiException:
                statusCode = apiException.StatusCode;
                errorCode = apiException.ErrorCode;
                message = apiException.Message;
                break;

            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                errorCode = "RESOURCE_NOT_FOUND";
                message = "The requested resource was not found.";
                break;

            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status403Forbidden;
                errorCode = "FORBIDDEN";
                message = "You are not authorized to perform this action.";
                break;

            case ValidationException ve:
                statusCode = StatusCodes.Status400BadRequest;
                errorCode = "VALIDATION_ERROR";
                message = ve.Message;
                break;

            case DbUpdateException:
                statusCode = StatusCodes.Status409Conflict;
                errorCode = "DATABASE_CONFLICT";
                message = "A database error occurred (e.g., duplicate data).";
                break;

            case ArgumentException ae:
                statusCode = StatusCodes.Status400BadRequest;
                errorCode = "INVALID_ARGUMENT";
                message = ae.Message;
                break;
        }

        var errorResponse = new
        {
            errorCode,
            message
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse), cancellationToken);

        return true;
    }
}