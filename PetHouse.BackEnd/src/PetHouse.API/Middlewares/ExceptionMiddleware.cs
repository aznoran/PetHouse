using PetHouse.API.Response;

namespace PetHouse.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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

            //здесь должно быть логирование, которое я прикручу после того как будет одобрен прошлый
            //pull request - в котором оно как раз и реализовано. TODO: logging with serilog
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}