using EventNucleus.Core.Primitives;
using System.Text.Json;

namespace EventNucleus.Presentation.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var (statusCode, error) = exception switch
        {
            DomainException domainEx => (StatusCodes.Status400BadRequest, 
                Error.Validation("Domain.Error", domainEx.Message)),
            KeyNotFoundException => (StatusCodes.Status404NotFound, 
                Error.NotFound("Resource.NotFound", "The requested resource was not found.")),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, 
                Error.Unauthorized("Auth.Unauthorized", "You are not authorized to perform this action.")),
            InvalidOperationException invEx when invEx.Message.Contains("not found") => 
                (StatusCodes.Status404NotFound, Error.NotFound("Resource.NotFound", invEx.Message)),
            _ => (StatusCodes.Status500InternalServerError, 
                Error.Failure("Server.Error", "An unexpected error occurred. Please try again later."))
        };

        context.Response.StatusCode = statusCode;

        var response = new
        {
            success = false,
            error = new
            {
                code = error.Code,
                message = error.Message,
                type = error.Type.ToString()
            },
            timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}

