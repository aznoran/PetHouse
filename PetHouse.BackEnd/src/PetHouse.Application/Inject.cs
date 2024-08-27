using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Volunteers.CreateVolunteer;

namespace PetHouse.Application;

public static class Inject
{
    public static IServiceCollection AddApplications(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateVolunteerHandler,CreateVolunteerHandler>();

        return serviceCollection;
    }
}