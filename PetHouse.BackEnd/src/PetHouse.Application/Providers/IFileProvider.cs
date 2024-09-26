﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHouse.Domain.Shared;
using PetHouse.Domain.Shared.ValueObjects;

namespace PetHouse.Application.Providers;

public interface IFileProvider
{
    
    Task<Result<IReadOnlyList<FileInfo>,Error>> UploadFiles(
        IEnumerable<FileData> files, 
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