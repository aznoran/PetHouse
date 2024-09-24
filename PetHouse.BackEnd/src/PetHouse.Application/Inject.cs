using FluentValidation;
using Scrutor;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Files.Delete;
using PetHouse.Application.Files.Get;
using PetHouse.Application.Files.GetAll;
using PetHouse.Application.Files.Upload;
using PetHouse.Application.Messaging;
using PetHouse.Application.Volunteers.AddPet;
using PetHouse.Application.Volunteers.AddPetPhoto;
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
        serviceCollection.AddCommandHandlerServices()
            //TODO: убрать когда придет время
            .AddFileHandlerServicesTESTING()
            .AddValidators();
        
        return serviceCollection;
    }

    
    private static IServiceCollection AddCommandHandlerServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(
                classes =>
                    classes.AssignableToAny(typeof(ICommandHandler<>), typeof(ICommandHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        return serviceCollection;
    }

    private static IServiceCollection AddFileHandlerServicesTESTING(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<FileUploadHandler>();
        serviceCollection.AddScoped<FileDeleteHandler>();
        serviceCollection.AddScoped<FileGetHandler>();
        serviceCollection.AddScoped<FileGetAllHandler>();
        return serviceCollection;
    }

    private static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return serviceCollection;
    }
}