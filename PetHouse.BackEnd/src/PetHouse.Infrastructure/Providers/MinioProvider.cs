using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetHouse.Application.Providers;
using PetHouse.Domain.Shared.Other;
using PetHouse.Domain.Shared.ValueObjects;
using FileInfo = PetHouse.Application.Providers.FileInfo;

namespace PetHouse.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    /*public async Task<UnitResult<Error>> Upload(Stream stream, string bucketName, Guid fileName, CancellationToken ct)
    {
        try
        {
            if (await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct) == false)
            {
                return Errors.File.BucketNotFound(bucketName);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName.ToString())
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("image/jpg");

            await _minioClient.PutObjectAsync(putObjectArgs, ct);

            _logger.LogInformation("Uploaded {FileName} successfully", fileName.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MinioProvider.Upload occured");
            return Errors.File.FailedToUpload();
        }

        return UnitResult.Success<Error>();
    }*/

    public async Task<Result<IReadOnlyList<FileInfo>, Error>> UploadFiles(IEnumerable<FileData> files, CancellationToken ct)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = files.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(filesList, ct);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, ct));
            
            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var results = pathsResult.Select(p => p.Value).ToList();

            _logger.LogInformation("Uploaded files: {files}", results.Select(f => f.Path.Value));

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MinioProvider.UploadFiles occured");
            return Errors.File.FailedToUpload();
        }
    }
    
    public async Task<UnitResult<Error>> DeleteFiles(string bucketName , IEnumerable<string> files, CancellationToken ct)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = files.ToList();

        try
        {
            if (await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct) == false)
            {
                return Errors.File.BucketNotFound(bucketName);
            }

            var tasks = filesList.Select(async file =>
                await RemoveObject(file, bucketName, semaphoreSlim, ct));
            
            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            _logger.LogInformation("Deleted files successfuly");

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MinioProvider.DeleteFiles occured");
            return Errors.File.FailedToDelete();
        }
    }

    public async Task<UnitResult<Error>> Delete(string bucketName, string fileName, CancellationToken ct)
    {
        try
        {
            if (await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct) == false)
            {
                return Errors.File.BucketNotFound(bucketName);
            }

            var obj = await _minioClient.StatObjectAsync(
                new StatObjectArgs().WithBucket(bucketName).WithObject(fileName), ct);

            if (obj is null)
            {
                return Errors.File.FileNotFound(fileName);
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, ct);

            _logger.LogInformation("Removed {FileName} successfully", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MinioProvider.Delete occured");
            return Errors.File.FailedToDelete(fileName);
        }

        return UnitResult.Success<Error>();
    }

    public async Task<Result<string, Error>> Get(string bucketName, string fileName, CancellationToken ct)
    {
        string? presignedUrl;
        try
        {
            if (await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct) == false)
            {
                return Errors.File.BucketNotFound(bucketName);
            }

            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithExpiry(1000);

            presignedUrl = await _minioClient.PresignedGetObjectAsync(args);

            _logger.LogInformation("Get {FileName} successfully with url: {Url}", fileName, presignedUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MinioProvider.Get occured");
            return Errors.File.FailedToGet(fileName);
        }

        return presignedUrl;
    }

    public async Task<Result<IReadOnlyList<string?>, Error>> GetAll(string bucketName,
        CancellationToken ct)
    {
        List<string?> presignedUrls = new List<string?>();
        try
        {
            if (await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct) == false)
            {
                return Errors.File.BucketNotFound(bucketName);
            }

            var listArgs = new ListObjectsArgs()
                .WithBucket(bucketName);
            await foreach (var item in _minioClient.ListObjectsEnumAsync(listArgs, ct))
            {
                var args = new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(item.Key)
                    .WithExpiry(1000);

                var presignedUrl = await _minioClient.PresignedGetObjectAsync(args);

                presignedUrls.Add(presignedUrl);
            }

            _logger.LogInformation("Listed all objects in bucket {bucketName}", bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MinioProvider.GetAll occured");
            return Errors.File.FailedToGet();
        }

        return presignedUrls;
    }

    private async Task<Result<FileInfo, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.FileInfo.BucketName)
            .WithStreamData(fileData.Content)
            .WithObjectSize(fileData.Content.Length)
            .WithObject(fileData.FileInfo.Path.Value);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FileInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FileInfo.Path.Value,
                fileData.FileInfo.BucketName);

            return Errors.File.FailedToUpload(fileData.FileInfo.Path.Value);
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    
    private async Task<UnitResult<Error>> RemoveObject(
        string fileName,
        string bucketName,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);

        try
        {
            await _minioClient
                .RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to delete file in minio with path {path} in bucket {bucket}",
                fileName,
                bucketName);

            return Errors.File.FailedToDelete(fileName);
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.FileInfo.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient
                .BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}