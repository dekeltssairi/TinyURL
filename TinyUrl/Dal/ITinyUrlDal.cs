using TinyUrl.Models;

namespace TinyUrl.Dal
{
    public interface ITinyUrlDal
    {
        public Task<UrlModel> InsertTinyUrl(Uri tinyUrl);
        public Task<UrlModel> GetOriginal(Uri tinyUrl);
    }
}
