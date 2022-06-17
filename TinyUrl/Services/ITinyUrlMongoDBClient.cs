using TinyUrl.Models;

namespace TinyUrl.Services
{
    public interface ITinyUrlMongoDBClient
    {
        public Task<UrlModel?> GetAsync(UrlModel TinyUrl);
        public Task <UrlModel> InsertAsync(UrlModel urlModel);

        //public Task RemoveAsync(string id);

    }
}
