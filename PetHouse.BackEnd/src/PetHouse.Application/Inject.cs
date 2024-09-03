using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Application.Volunteers.Delete;

namespace PetHouse.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateVolunteerHandler,CreateVolunteerHandler>();
        serviceCollection.AddScoped<IDeleteVolunteerHandler, DeleteVolunteerHandler>();
        serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return serviceCollection;
    }
}