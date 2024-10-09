using PetHouse.Core.Models;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var error = Error.Failure("server.internal", e.Message);
            var envelope = Envelope.Error(error);

            _logger.LogError(e, "Server error :{ResponseError}",error.Message);
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}