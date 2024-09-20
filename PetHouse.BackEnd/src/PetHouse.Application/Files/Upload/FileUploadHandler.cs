using CSharpFunctionalExtensions;
using PetHouse.Application.Providers;
using PetHouse.Application.Volunteers.Create;
using PetHouse.Domain.Shared;

namespace PetHouse.Application.Files.Upload;

public class FileUploadHandler
{
    private readonly IFileProvider _fileProvider;

    public FileUploadHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    /*public async Task<UnitResult<Error>> Handle(Stream stream, string bucketName, CancellationToken ct)
    {
        var uploadRes = await _fileProvider.Upload(stream, bucketName,Guid.NewGuid(), ct);

        return uploadRes.IsFailure ? uploadRes.Error : UnitResult.Success<Error>();
    }*/
}