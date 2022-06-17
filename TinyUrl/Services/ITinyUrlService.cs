namespace TinyUrl.Services
{
    public interface ITinyUrlService
    {
        public Task CreateTinyUrl(Uri url);
    }
}
