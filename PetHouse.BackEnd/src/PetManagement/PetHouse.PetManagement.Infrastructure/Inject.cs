using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetHouse.Core.Messaging;
using PetHouse.Core.Providers;
using PetHouse.PetManagement.Application;
using PetHouse.PetManagement.Infrastructure.BackgroundServices;
using PetHouse.PetManagement.Infrastructure.Data;
using PetHouse.PetManagement.Infrastructure.Files;
using PetHouse.PetManagement.Infrastructure.Messaging;
using PetHouse.PetManagement.Infrastructure.Options;
using PetHouse.PetManagement.Infrastructure.Providers;
using PetHouse.PetManagement.Infrastructure.Repositories;
using FileInfo = PetHouse.Core.Providers.FileInfo;
using IFileProvider = PetHouse.Core.Providers.IFileProvider;

namespace PetHouse.PetManagement.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddPetManagementInfrastructure(this IServiceCollection serviceCollection,
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