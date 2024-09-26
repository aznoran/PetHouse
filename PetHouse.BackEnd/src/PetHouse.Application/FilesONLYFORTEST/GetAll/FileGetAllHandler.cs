using CSharpFunctionalExtensions;
using PetHouse.Application.Providers;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.FilesONLYFORTEST.GetAll;

public class FileGetAllHandler
{
    private readonly IFileProvider _fileProvider;

    public FileGetAllHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<IReadOnlyList<string?>, Error>> Handle(string bucketName, CancellationToken ct)
    {
        var uploadRes = await _fileProvider.GetAll(bucketName, ct);

        if (uploadRes.IsFailure)
        {
            return uploadRes.Error;
        }

        return Result.Success<IReadOnlyList<string?>, Error>(uploadRes.Value);
    }
}