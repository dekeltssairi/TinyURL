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
        private readonly IUrlMemoryCache _cache;

        public TinyUrlDal(ITinyUrlMongoDBClient tinyUrlMongoDBClient, IUrlMemoryCache urlMemoryCache)
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

            _cache.InsertIfNotExist(urlModel);

            if (_cache.IsFull())
            {
                await UpdateDbWithCacheValues();
            }

            return urlModel.TinyUrl;
        }

        private async Task UpdateDbWithCacheValues()
        {
            IEnumerable<UrlModel>? entites = _cache.OrginalUrlToModelDict.Values.AsEnumerable();
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
                    _cache.InsertIfNotExist(urlModelFromDb);

                    if (_cache.OrginalUrlToModelDict.Count == _cache.MaxCacheSize)
                    {
                        await UpdateDbWithCacheValues();
                    }

                    return urlModelFromDb.OriginalUrl;
                }
            }

            return result?.OriginalUrl;
        }
    }
}
