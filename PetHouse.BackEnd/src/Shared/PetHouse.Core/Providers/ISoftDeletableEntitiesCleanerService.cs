namespace PetHouse.PetManagement.Infrastructure.Services;

public interface ISoftDeletableEntitiesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}