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
        private readonly ITinyUrlMongoDBClient _tinyUrlMongoDBClient;
        private readonly IUrlMemoryCache _cache;

        public TinyUrlDal(ITinyUrlMongoDBClient tinyUrlMongoDBClient, IUrlMemoryCache urlMemoryCache)
        {
            _tinyUrlMongoDBClient = tinyUrlMongoDBClient ?? throw new ArgumentNullException(nameof(tinyUrlMongoDBClient));
            _cache = urlMemoryCache ?? throw new ArgumentNullException(nameof(urlMemoryCache));
        }

        public async Task<Uri> InsertTinyUrl(Uri orginalUrl, Uri tinyUrl)
        {
            UrlModel urlModel = new UrlModel()
            {
                OriginalUrl = orginalUrl,
                TinyUrl = tinyUrl
            };

            UrlModel? result = _cache.GetOrCreate(urlModel.OriginalUrl, urlModel);

            if (_cache.KeyValuePairs.Count == _cache.MaxCacheSize)
            {
                await UpdateDbWithCacheValues();
            }

            return result?.TinyUrl;
        }

        private async Task UpdateDbWithCacheValues()
        {
            IEnumerable<UrlModel>? entites = _cache.KeyValuePairs.Values.AsEnumerable();
            await _tinyUrlMongoDBClient.UpsertManyAsync(entites);
            _cache.Clear();
        }

        public async Task<Uri> GetOriginal(Uri tinyUrl)
        {

            UrlModel? result = _cache.Get(tinyUrl);

            if (result is null)
            {
                UrlModel urlModel = new UrlModel()
                {
                    TinyUrl = tinyUrl
                };

                UrlModel urlModelFromDb =  await _tinyUrlMongoDBClient.GetAsync(urlModel);

                if (urlModelFromDb != null)
                {
                    result = urlModelFromDb;

                    _cache.GetOrCreate(urlModelFromDb.OriginalUrl, urlModelFromDb);

                    if (_cache.KeyValuePairs.Count == _cache.MaxCacheSize)
                    {
                        await UpdateDbWithCacheValues();
                    }
                }
            }


            return result?.OriginalUrl;
        }
    }
}
