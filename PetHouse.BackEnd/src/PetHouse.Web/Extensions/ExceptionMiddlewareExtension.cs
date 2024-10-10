using PetHouse.Web.Middlewares;

namespace PetHouse.Web.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionLogMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}