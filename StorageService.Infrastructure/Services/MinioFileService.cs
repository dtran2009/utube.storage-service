using Minio;
using StorageService.Application.Services;
using StorageService.Infrastructure.Settings;

namespace StorageService.Infrastructure.Services;

internal class MinioFileService : IFileService
{
    private readonly MinioClient _minioClient;
    private readonly MinioSetting _minioSetting;

    public MinioFileService(MinioSetting minioSetting, MinioClient minioClient)
    {
        _minioSetting = minioSetting;
        _minioClient = minioClient;
    }

    public async Task<string> UploadFileAsync(string id, Stream stream, string filename, string mimeType, CancellationToken cancellationToken = default)
    {
        await CreateBucketIfNotExistsAsync(_minioSetting.BucketName, cancellationToken).ConfigureAwait(false);

        var putObjectArgs = new PutObjectArgs()
                .WithBucket(_minioSetting.BucketName)
                .WithObject(filename)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(mimeType);

        var response = await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

        return response.ObjectName;
    }

    private async Task CreateBucketIfNotExistsAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        var bucketExists = await CheckIfBucketExistsAsync(bucketName, cancellationToken);

        if (bucketExists)
        {
            return;
        }

        var beArgs = new MakeBucketArgs().WithBucket(bucketName);
        await _minioClient.MakeBucketAsync(beArgs, cancellationToken).ConfigureAwait(false);
    }

    private async Task<bool> CheckIfBucketExistsAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        var beArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

        return await _minioClient.BucketExistsAsync(beArgs, cancellationToken);
    }
}
