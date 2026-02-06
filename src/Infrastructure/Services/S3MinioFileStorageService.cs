using Application.Abstracts.Services;
using Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Services;

public class S3MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _options;
    private readonly ILogger<S3MinioFileStorageService> _logger;

    public S3MinioFileStorageService(IMinioClient minioClient, IOptions<MinioOptions> options, ILogger<S3MinioFileStorageService> logger)
    {
        _minioClient = minioClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> SaveAsync(Stream content, string fileName, string contentType, int propertyAdId, CancellationToken ct = default)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(_options.Bucket);
            bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs, ct);
            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(_options.Bucket);
                await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
            }

            var objectKey = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_options.Bucket)
                .WithObject(objectKey)
                .WithStreamData(content)
                .WithObjectSize(content.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs, ct);

            return objectKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to MinIO");
            throw;
        }
    }

    public async Task DeleteFileAsync(string objectKey, CancellationToken ct = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_options.Bucket)
                .WithObject(objectKey);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from MinIO");
            throw;
        }
    }

    public string GetFileUrl(string fileName)
    {
        // Construct the URL manually or use PresignedGetObjectAsync if private access is needed.
        // For public buckets: http://endpoint/bucket/filename
        var protocol = _options.UseSSL ? "https" : "http";
        return $"{protocol}://{_options.Endpoint}/{_options.Bucket}/{fileName}";
    }
}
