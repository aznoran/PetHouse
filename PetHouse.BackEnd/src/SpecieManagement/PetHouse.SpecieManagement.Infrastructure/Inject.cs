using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHouse.Core.Messaging;
using PetHouse.Core.Providers;
using PetHouse.SpecieManagement.Application;
using PetHouse.SpecieManagement.Application.SpecieManagement;
using PetHouse.SpecieManagement.Infrastructure.Data;
using PetHouse.SpecieManagement.Infrastructure.Repositories;
using FileInfo = PetHouse.Core.Providers.FileInfo;
using IFileProvider = PetHouse.Core.Providers.IFileProvider;

namespace PetHouse.SpecieManagement.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddSpecieManagementInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection
            .AddServices()
            .AddDataBaseLogic();
        
        return serviceCollection;
    }

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return serviceCollection;
    }

    private static IServiceCollection AddDataBaseLogic(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<WriteDbContext>();
        serviceCollection.AddScoped<IReadDbContext, ReadDbContext>();
        serviceCollection.AddScoped<ISpecieRepository, SpecieRepository>();
        
        return serviceCollection;
    }
}