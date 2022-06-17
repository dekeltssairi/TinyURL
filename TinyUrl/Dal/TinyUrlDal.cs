using MongoDB.Bson;
using TinyUrl.Models;
using TinyUrl.Services;

namespace TinyUrl.Dal
{
    public class TinyUrlDal : ITinyUrlDal
    {
        private readonly ITinyUrlMongoDBClient _tinyUrlMongoDBClient;

        public TinyUrlDal(ITinyUrlMongoDBClient tinyUrlMongoDBClient)
        {
            _tinyUrlMongoDBClient = tinyUrlMongoDBClient ?? throw new ArgumentNullException(nameof(tinyUrlMongoDBClient));
        }

        public async Task<UrlModel> InsertTinyUrl(Uri tinyUrl)
        {
            UrlModel urlModel = new UrlModel()
            {
                OriginalUrl = tinyUrl,
            };

            return await _tinyUrlMongoDBClient.InsertAsync(urlModel);
        }

        public async Task<UrlModel> GetOriginal(Uri tinyUrl)
        {
            ObjectId objectId = ObjectId.Parse(tinyUrl.LocalPath[1..]);

            UrlModel urlModel = new UrlModel()
            {
                _id = objectId
            };

            return await _tinyUrlMongoDBClient.GetAsync(urlModel);
        }
    }
}
