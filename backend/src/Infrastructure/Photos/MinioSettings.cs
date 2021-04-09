namespace Infrastructure.Photos
{
    public class MinioSettings
    {
        public string Endpoint { get; set; }
        public string AccessPoint {get; set;}
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string BucketName { get; set; }
    }
}