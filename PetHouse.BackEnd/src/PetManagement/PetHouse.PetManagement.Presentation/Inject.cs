using Microsoft.Extensions.DependencyInjection;
using PetHouse.PetManagement.Contracts;

namespace PetHouse.PetManagement.Presentation;

public static class Inject
{
    public static IServiceCollection AddPetManagementPresentation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IPetManagementContract, PetManagementContract>();
        
        return serviceCollection;
    }
}