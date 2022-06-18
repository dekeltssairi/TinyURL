using System.Collections.Generic;
using TinyUrl.Models;

namespace TinyUrl.Services
{
    public interface ITinyUrlMongoDBClient
    {
        public Task<UrlModel?> GetAsync(UrlModel TinyUrl);
        public Task UpsertManyAsync(IEnumerable<UrlModel> urlModels);
    }
}
