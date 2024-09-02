using PetHouse.API.Validation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Serilog;

namespace PetHouse.API;

public static class Inject
{
    public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();
        serviceCollection.AddControllers();
        serviceCollection.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
        serviceCollection.AddSerilog();
        
        return serviceCollection;
    }
}