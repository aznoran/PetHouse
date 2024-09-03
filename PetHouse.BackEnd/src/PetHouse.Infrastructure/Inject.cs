using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Volunteers;
using PetHouse.Infrastructure.Repositories;

namespace PetHouse.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IVolunteersRepository, VolunteersRepository>();
        
        return serviceCollection;
    }
}