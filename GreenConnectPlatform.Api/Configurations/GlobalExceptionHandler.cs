using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using GreenConnectPlatform.Business.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

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
        var errorCode = "PR50001";
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
                errorCode = "PR40401";
                message = "The requested resource was not found.";
                break;

            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status403Forbidden;
                errorCode = "PR40301";
                message = "You are not authorized to perform this action.";
                break;

            case ValidationException ve:
                statusCode = StatusCodes.Status400BadRequest;
                errorCode = "PR40001";
                message = ve.Message;
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