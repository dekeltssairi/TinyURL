using TinyUrl.Models;

namespace TinyUrl.Dal
{
    public interface ITinyUrlDal
    {
        public Task<Uri> InsertTinyUrl(Uri orginalUrl, Uri tinyUrl);
        public Task<Uri> GetOriginal(Uri tinyUrl);
    }
}
