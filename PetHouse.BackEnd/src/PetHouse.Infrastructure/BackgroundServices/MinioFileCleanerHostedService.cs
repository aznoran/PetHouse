using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetHouse.Infrastructure.Files;

namespace PetHouse.Infrastructure.BackgroundServices;

public class MinioFileCleanerHostedService : BackgroundService
{
    private readonly ILogger<MinioFileCleanerHostedService> _logger;

    public MinioFileCleanerHostedService(
        IServiceProvider services, 
        ILogger<MinioFileCleanerHostedService> logger)
    {
        Services = services;
        _logger = logger;
    }
    
    public IServiceProvider Services { get; }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Started running MinioFileCLeanerHostedService.");

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        await using (var scope = Services.CreateAsyncScope())
        {
            var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFilesCleanerService>();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await filesCleanerService.Process(stoppingToken);
            }
        
            await Task.CompletedTask;
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}