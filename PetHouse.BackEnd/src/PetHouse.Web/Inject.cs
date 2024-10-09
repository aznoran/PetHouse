using Serilog;

namespace PetHouse.Web;

public static class Inject
{
    public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();
        serviceCollection.AddControllers();
                
        serviceCollection.AddSerilog();
        
        return serviceCollection;
    }
}