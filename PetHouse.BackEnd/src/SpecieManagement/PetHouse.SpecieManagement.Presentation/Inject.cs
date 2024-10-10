using Microsoft.Extensions.DependencyInjection;
using PetHouse.SpecieManagement._Contracts;

namespace PetHouse.SpecieManagement.Presentation;

public static class Inject
{
    public static IServiceCollection AddSpecieManagementPresentation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISpecieManagementContract, SpecieManagementContract>();
        
        return serviceCollection;
    }
}