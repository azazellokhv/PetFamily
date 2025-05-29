using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Files;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLEL_FILES = 5;
    private const int EXPIRY_VALUE = 60 * 60 * 24;

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLEL_FILES);
        var filesList = filesData.ToList();

        try
        {
            await IfBucketsNotExistCreateBuckets(filesList
                .Select(file => file.FileInfo.BucketName), cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathResult = await Task.WhenAll(tasks);

            if (pathResult.Any(pr => pr.IsFailure))
                return pathResult.First().Error;

            var result = pathResult.Select(pr => pr.Value).ToList();

            _logger.LogInformation("Uploaded files {files}", result.Select(f => f.Path));

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }

    /*public async Task<Result<string, Error>> RemoveFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = IsBucketExist(fileInfo.BucketName, cancellationToken).Result;
            if (bucketExist == false)
                return Error.Failure("bucket.not.found", "Bucket not found");

            var objectExistArgs = new PresignedGetObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.Path);

            var objectExistResult = await _minioClient.PresignedGetObjectAsync(objectExistArgs);
            if (string.IsNullOrWhiteSpace(objectExistResult))
                return Error.NotFound("file.not.found", "File not found");

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.Path);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return fileInfo.FilePath.Path;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to remove file from minio {path} in bucket {bucket}",
                fileInfo.FilePath,
                fileInfo.BucketName);
            
            return Error.Failure("file.delete", "Fail to delete file from minio");
        }
    }*/

    public async Task<UnitResult<Error>> RemoveFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await IfBucketsNotExistCreateBuckets([fileInfo.BucketName], cancellationToken);
            
            var statArgs = new StatObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.Path);
            
            var objectStat = _minioClient.StatObjectAsync(statArgs, cancellationToken);
            if (objectStat is null)
                return Result.Success<Error>();
            
            var removeArgs = new RemoveObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.Path);
            
            await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
            
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "Fail to remove file from minio with path {path} in bucket {bucket}",
                fileInfo.FilePath.Path,
                fileInfo.BucketName);
            
            return Error.Failure("file.delete", "Fail to delete file from minio");
        }

        return Result.Success<Error>();
    }

    public async Task<Result<string, Error>> GetUrlFileByName(
        FileMetaData fileMetaData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = IsBucketExist(fileMetaData.BucketName, cancellationToken).Result;
            if (bucketExist == false)
                return Error.Failure("bucket.not.found", "Bucket not found");

            var objectExistArgs = new PresignedGetObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName);

            var objectExistUrl = await _minioClient.PresignedGetObjectAsync(objectExistArgs);
            if (string.IsNullOrWhiteSpace(objectExistUrl))
                return Error.NotFound("file.not.found", "File not found");

            return objectExistUrl;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to download file in minio");
            return Error.Failure("file.download", "Fail to download file in minio");
        }
    }

    private async Task IfBucketsNotExistCreateBuckets(
        IEnumerable<string> buckets,
        CancellationToken cancellationToken = default)
    {
        HashSet<string> bucketNames = [..buckets];

        foreach (var bucketName in bucketNames)
        {
            var bucketExist = await IsBucketExist(bucketName, cancellationToken);

            if (bucketExist == false)
            {
                await CreateBucket(bucketName, cancellationToken);
            }
        }
    }

    private async Task<bool> IsBucketExist(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        var bucketExist = await _minioClient
            .BucketExistsAsync(bucketExistArgs, cancellationToken);

        return bucketExist;
    }

    private async Task CreateBucket(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(bucketName);

        await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
    }

    private async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.FileInfo.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FileInfo.FilePath.Path);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FileInfo.FilePath;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to upload file with path {path} in bucket {bucket}",
                fileData.FileInfo.FilePath.Path, 
                fileData.FileInfo.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
}