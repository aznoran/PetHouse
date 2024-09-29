using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetHouse.Application.Abstraction;
using PetHouse.Application.Messaging;
using PetHouse.Application.PetManagement;
using PetHouse.Application.Providers;
using PetHouse.Application.SpecieManagment;
using PetHouse.Domain.Shared.Other;
using PetHouse.Infrastructure.BackgroundServices;
using PetHouse.Infrastructure.Data;
using PetHouse.Infrastructure.Files;
using PetHouse.Infrastructure.Messaging;
using PetHouse.Infrastructure.Options;
using PetHouse.Infrastructure.Providers;
using PetHouse.Infrastructure.Repositories;
using FileInfo = PetHouse.Application.Providers.FileInfo;
using IFileProvider = PetHouse.Application.Providers.IFileProvider;

namespace PetHouse.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddMinioServices(configuration)
            .AddProviders()
            .AddServices()
            .AddDataBaseLogic()
            .AddHostedServices()
            .AddMessaging();
        
        return serviceCollection;
    }

    private static IServiceCollection AddMinioServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));

        serviceCollection.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                .WithEndpoint(minioOptions.Endpoint)
                .WithSSL(minioOptions.IsSsl);
        });

        return serviceCollection;
    }

    private static IServiceCollection AddProviders(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFileProvider, MinioProvider>();

        return serviceCollection;
    }

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IFilesCleanerService, FilesCleanerService>();
        
        return serviceCollection;
    }

    private static IServiceCollection AddDataBaseLogic(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<PetHouseWriteDbContext>();
        serviceCollection.AddScoped<IReadDbContext, PetHouseReadDbContext>();
        serviceCollection.AddScoped<IVolunteersRepository, VolunteersRepository>();
        serviceCollection.AddScoped<ISpecieRepository, SpecieRepository>();
        
        return serviceCollection;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<MinioFileCleanerHostedService>();

        return serviceCollection;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>,
            InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        return serviceCollection;
    }
}