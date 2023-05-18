namespace StorageService.Application.Setting
{
    public class StorageSetting
    {
        public string HostName { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
    }

    public static class StorageExtension
    {
        public static string GetObjectUrl(this StorageSetting storageSetting, string objectPath)
        {
            string url = string.Empty;

            if (storageSetting is not null)
            {
                url = $"{storageSetting.HostName}/{storageSetting.BucketName}/{objectPath}";
            }

            return url;
        }
    }
}
