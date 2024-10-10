namespace PetHouse.Core.Providers;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}