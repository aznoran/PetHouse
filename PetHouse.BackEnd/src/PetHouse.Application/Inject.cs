using FluentValidation;
using Scrutor;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Application.Abstraction;
using PetHouse.Application.FilesONLYFORTEST.Delete;
using PetHouse.Application.FilesONLYFORTEST.Get;
using PetHouse.Application.FilesONLYFORTEST.GetAll;
using PetHouse.Application.FilesONLYFORTEST.Upload;

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
        
        serviceCollection.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(
                classes =>
                    classes.AssignableToAny(typeof(IQueryHandler<,>)))
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