
namespace StorageService.Infrastructure.Settings;

public class MinioSetting
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string Secretkey { get; set; } = string.Empty;
    public bool UseSSL { get; set; } = false;
    public bool HttpTrace { get; set; } = false;
    public string BucketName { get; set; } = string.Empty;
}
