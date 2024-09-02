using PetHouse.API.Response;

namespace PetHouse.API.Middlewares;

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

            ResponseError responseError = new ResponseError("server.internal", e.Message, null);
            var envelope = Envelope.Error([responseError]);

            _logger.LogError(e, "Server error :{ResponseError}",responseError.ErrorMessage);
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}