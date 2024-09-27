namespace PetHouse.Application.Providers;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}