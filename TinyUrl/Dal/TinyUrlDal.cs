using MongoDB.Bson;
using System.Linq;
using System.Text.Json;
using TinyUrl.Cache;
using TinyUrl.Models;
using TinyUrl.Services;

namespace TinyUrl.Dal
{
    public class TinyUrlDal : ITinyUrlDal
    {
        private static readonly object _locker = new object();
        private readonly ITinyUrlMongoDBClient _tinyUrlMongoDBClient;
        private readonly ICache _cache;

        public TinyUrlDal(ITinyUrlMongoDBClient tinyUrlMongoDBClient, ICache urlMemoryCache)
        {
            _tinyUrlMongoDBClient = tinyUrlMongoDBClient ?? throw new ArgumentNullException(nameof(tinyUrlMongoDBClient));
            _cache = urlMemoryCache ?? throw new ArgumentNullException(nameof(urlMemoryCache));
        }

        public async Task<Uri> InsertTinyUrl(Uri originalUrl, Uri tinyUrl)
        {
            UrlModel urlModel = new UrlModel()
            {
                OriginalUrl = originalUrl,
                TinyUrl = tinyUrl
            };

            _cache.Put(urlModel.TinyUrl, urlModel.OriginalUrl);
            await _tinyUrlMongoDBClient.UpsertAsync(urlModel);

            return urlModel.TinyUrl;
        }

        //private async Task UpdateDbWithCacheValues()
        //{
        //    Dictionary<Uri, (LinkedListNode<Uri> node, Uri value, int count)>.ValueCollection? res = _cache._cache.Values;

        //    foreach ((LinkedListNode<Uri> node, Uri value, int count) item in res)
        //    {
        //        Uri? res2 = item.value;
        //    }

        //    await _tinyUrlMongoDBClient.UpsertManyAsync(entites);
        //    _cache.Clear();
        //}

        public async Task<Uri> GetOriginal(Uri tinyUrl)
        {

            Uri? result = _cache.Get(tinyUrl);

            if (result is null)
            {
                UrlModel urlModel = new UrlModel()
                {
                    TinyUrl = tinyUrl
                };

                UrlModel? urlModelFromDb =  await _tinyUrlMongoDBClient.GetAsync(urlModel);

                if (urlModelFromDb != null)
                {
                    _cache.Put(urlModel.TinyUrl, urlModel.OriginalUrl);
                    return urlModelFromDb.OriginalUrl;
                }
            }

            return result;
        }
    }
}
