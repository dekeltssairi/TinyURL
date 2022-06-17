namespace TinyUrl.Models
{
    public class UrlsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UrlsCollectionName { get; set; } = null!;
    }
}
