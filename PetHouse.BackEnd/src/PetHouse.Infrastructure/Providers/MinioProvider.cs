using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetHouse.Application.Providers;
using PetHouse.Domain.Shared;

namespace PetHouse.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> Upload(Stream stream, string bucketName, Guid fileName, CancellationToken ct)
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
    }

    public async Task<UnitResult<Error>> Delete(string bucketName, string fileName, CancellationToken ct)
    {
        try
        {
            if (await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct) == false)
            {
                return Errors.File.BucketNotFound(bucketName);
            }

            var obj = await _minioClient.StatObjectAsync(new StatObjectArgs().WithBucket(bucketName).WithObject(fileName), ct);

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
}