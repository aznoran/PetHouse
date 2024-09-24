namespace PetHouse.Infrastructure.Files;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}