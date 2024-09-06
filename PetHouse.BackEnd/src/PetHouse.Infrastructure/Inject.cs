using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetHouse.Application.Volunteers;
using PetHouse.Infrastructure.Options;
using PetHouse.Infrastructure.Providers;
using PetHouse.Infrastructure.Repositories;
using IFileProvider = PetHouse.Application.Providers.IFileProvider;

namespace PetHouse.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddScoped<IVolunteersRepository, VolunteersRepository>();

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

        serviceCollection.AddScoped<IFileProvider, MinioProvider>();
        
        return serviceCollection;
    }
}