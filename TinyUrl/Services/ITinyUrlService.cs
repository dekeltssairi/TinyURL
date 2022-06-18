using TinyUrl.Models;

namespace TinyUrl.Services
{
    public interface ITinyUrlService
    {
        public Task<Uri> CreateTinyUrl(Uri url);

        public Task<Uri> GetOriginalUrl(Uri tinyUrl);
    }
}
