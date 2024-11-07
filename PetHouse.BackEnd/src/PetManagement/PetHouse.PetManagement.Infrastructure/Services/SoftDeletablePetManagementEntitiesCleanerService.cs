using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.PetManagement.Infrastructure.Data;
using PetHouse.SharedKernel.Id;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.PetManagement.Infrastructure.Services;

public class SoftDeletablePetManagementEntitiesCleanerService : ISoftDeletableEntitiesCleanerService
{
    private readonly ILogger<SoftDeletablePetManagementEntitiesCleanerService> _logger;
    private readonly WriteDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private const string SOFT_DELETE_DAYS = "SoftDeleteDays";

    public SoftDeletablePetManagementEntitiesCleanerService(
        ILogger<SoftDeletablePetManagementEntitiesCleanerService> logger,
        WriteDbContext dbContext,
        IConfiguration configuration)
    {
        _logger = logger;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        await DeleteVolunteers(cancellationToken);
        _logger.LogInformation("BackGroundService: Deleted Volunteers");
        await DeletePets(cancellationToken);
        _logger.LogInformation("BackGroundService: Deleted Pets");
    }

    private async Task DeleteVolunteers(CancellationToken cancellationToken)
    {
        var entities = await _dbContext
            .Set<SoftDeletableEntity<VolunteerId>>()
            .Where(x => x.IsDeleted)
            .ToListAsync(cancellationToken);
        
        foreach (var entity in entities)
        {
            if (entity.DeletedTime is not null && entity.DeletedTime.Value
                    .AddDays(double.Parse(_configuration.GetSection(SOFT_DELETE_DAYS).Value!)) < DateTime.UtcNow)
            {
                _dbContext.Remove(entity);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task DeletePets(CancellationToken cancellationToken)
    {
        var entities = await _dbContext
            .Set<SoftDeletableEntity<PetId>>()
            .Where(x => x.IsDeleted)
            .ToListAsync(cancellationToken);
        
        foreach (var entity in entities)
        {
            if (entity.DeletedTime is not null && entity.DeletedTime.Value
                    .AddDays(double.Parse(_configuration.GetSection(SOFT_DELETE_DAYS).Value!)) < DateTime.UtcNow)
            {
                _dbContext.Remove(entity);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}