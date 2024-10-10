using Microsoft.Extensions.Logging;
using PetHouse.Core.Messaging;
using PetHouse.Core.Providers;
using FileInfo = PetHouse.Core.Providers.FileInfo;

namespace PetHouse.PetManagement.Infrastructure.Files;

public class FilesCleanerService : IFilesCleanerService
{
    private readonly ILogger<FilesCleanerService> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IFileProvider _fileProvider;

    public FilesCleanerService(
        ILogger<FilesCleanerService> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IFileProvider fileProvider
    )
    {
        _logger = logger;
        _messageQueue = messageQueue;
        _fileProvider = fileProvider;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var files = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var file in files)
        {
            await _fileProvider.Delete(file.BucketName, file.Path.Value, cancellationToken);
        }
    }
}