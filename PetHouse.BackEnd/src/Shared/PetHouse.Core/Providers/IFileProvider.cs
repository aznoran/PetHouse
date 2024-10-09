using CSharpFunctionalExtensions;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Core.Providers;

public interface IFileProvider
{
    
    Task<Result<IReadOnlyList<FileInfo>,Error>> UploadFiles(
        IEnumerable<FileData> files, 
        CancellationToken ct);

    Task<UnitResult<Error>> DeleteFiles(
        string bucketName,
        IEnumerable<string> files,
        CancellationToken ct);
    
    Task<UnitResult<Error>> Delete(
        string bucketName,
        string fileName,
        CancellationToken ct);

    Task<Result<string, Error>> Get(
        string bucketName,
        string fileName,
        CancellationToken ct);

    Task<Result<IReadOnlyList<string?>, Error>> GetAll(
        string bucketName,
        CancellationToken ct);
}