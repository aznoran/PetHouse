using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Application.Volunteers.Delete;
using PetHouse.Application.Volunteers.UpdateMainInfo;
using PetHouse.Application.Volunteers.UpdateRequisites;
using PetHouse.Application.Volunteers.UpdateSocialNetworks;

namespace PetHouse.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateVolunteerHandler,CreateVolunteerHandler>();
        serviceCollection.AddScoped<IDeleteVolunteerHandler, DeleteVolunteerHandler>();
        serviceCollection.AddScoped<IUpdateVolunteerMainInfoHandler,UpdateVolunteerMainInfoHandler>();
        serviceCollection.AddScoped<IUpdateVolunteerRequisitesHandler,UpdateVolunteerRequisitesHandler>();
        serviceCollection.AddScoped<IUpdateVolunteerSocialNetworksHandler,UpdateVolunteerSocialNetworksHandler>();
        serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return serviceCollection;
    }
}