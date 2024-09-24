using Microsoft.Extensions.Logging;
using PetHouse.Application.Messaging;
using PetHouse.Application.Providers;
using FileInfo = PetHouse.Application.Providers.FileInfo;

namespace PetHouse.Infrastructure.Files;

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