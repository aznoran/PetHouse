using CSharpFunctionalExtensions;
using PetHouse.Application.Providers;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Files.Delete;

public class FileDeleteHandler
{
    private readonly IFileProvider _fileProvider;

    public FileDeleteHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<Error>> Handle(string bucketName, string fileName, CancellationToken ct)
    {
        var uploadRes = await _fileProvider.Delete(bucketName, fileName, ct);

        return uploadRes.IsFailure ? uploadRes.Error : UnitResult.Success<Error>();
    }
}