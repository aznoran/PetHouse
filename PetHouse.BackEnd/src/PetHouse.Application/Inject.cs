using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Volunteers.CreateVolunteer;

namespace PetHouse.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateVolunteerHandler,CreateVolunteerHandler>();

        return serviceCollection;
    }
}