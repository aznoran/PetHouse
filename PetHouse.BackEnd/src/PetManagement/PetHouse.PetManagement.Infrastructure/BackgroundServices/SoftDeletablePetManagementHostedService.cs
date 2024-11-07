using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetHouse.PetManagement.Infrastructure.Services;

namespace PetHouse.PetManagement.Infrastructure.BackgroundServices;

public class SoftDeletablePetManagementHostedService : BackgroundService
{
    private const int DELETE_SOFTDELETABLEENTITIES_SERVICE_REDUCTION_HOURS = 24;
    private readonly ILogger<SoftDeletablePetManagementHostedService> _logger;

    public SoftDeletablePetManagementHostedService(
        IServiceProvider services, 
        ILogger<SoftDeletablePetManagementHostedService> logger)
    {
        Services = services;
        _logger = logger;
    }
    
    public IServiceProvider Services { get; }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Started running SoftDeletableHostedService.");

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        await using (var scope = Services.CreateAsyncScope())
        {
            var softDeletebleService = scope.ServiceProvider.GetRequiredService<ISoftDeletableEntitiesCleanerService>();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await softDeletebleService.Process(stoppingToken);

                await Task
                    .Delay(TimeSpan.FromHours(DELETE_SOFTDELETABLEENTITIES_SERVICE_REDUCTION_HOURS), stoppingToken);
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