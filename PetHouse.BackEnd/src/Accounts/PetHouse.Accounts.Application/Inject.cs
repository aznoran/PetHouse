using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Core.Abstraction;

namespace PetHouse.Accounts.Application;

public static class Inject
{
    public static IServiceCollection AddAccountsApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCommandHandlerServices()
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

    private static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return serviceCollection;
    }
}