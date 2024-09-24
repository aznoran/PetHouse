using CSharpFunctionalExtensions;
using PetHouse.Application.Providers;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Files.Get;

public class FileGetHandler
{
    private readonly IFileProvider _fileProvider;

    public FileGetHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string?,Error>> Handle(string bucketName, string fileName, CancellationToken ct)
    {
        var uploadRes = await _fileProvider.Get(bucketName, fileName, ct);

        return uploadRes.IsFailure ? uploadRes.Error : uploadRes.Value;
    }
}