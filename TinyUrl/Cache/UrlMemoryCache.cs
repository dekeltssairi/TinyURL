using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TinyUrl.Models;

namespace TinyUrl.Cache
{
    public class UrlMemoryCache : IUrlMemoryCache
    {
        public int? MaxCacheSize { get; private set; }

        public Dictionary<Uri, UrlModel> OrginalUrlToModelDict  { get; private set; }
        public Dictionary<Uri, UrlModel> TinyUrlToModelDict  { get; private set; }


        public UrlMemoryCache(IOptions<CacheSettings> CacheSettings)
        {
            MaxCacheSize = CacheSettings.Value.MaxCacheSize ?? throw new ArgumentNullException(nameof(CacheSettings.Value.MaxCacheSize));
            OrginalUrlToModelDict = new Dictionary<Uri, UrlModel>();
            TinyUrlToModelDict = new Dictionary<Uri, UrlModel>();
        }

        public void InsertIfNotExist(UrlModel urlModel)
        {
            lock (Locker._locker)
            {
                if (!OrginalUrlToModelDict.ContainsKey(urlModel.OriginalUrl))
                {
                    OrginalUrlToModelDict[urlModel.OriginalUrl] = urlModel;
                    TinyUrlToModelDict[urlModel.TinyUrl] = urlModel;
                }
            }
        }

        public UrlModel Get(Uri TinyUriKey)
        {
            lock (Locker._locker)
            {
                return TinyUrlToModelDict.ContainsKey(TinyUriKey) ? TinyUrlToModelDict[TinyUriKey] : default;
            }
        }

        public void Clear()
        {
            lock(Locker._locker)
            {
                OrginalUrlToModelDict.Clear();
                TinyUrlToModelDict.Clear();
            }
        }

        public bool IsFull()
        {
            return OrginalUrlToModelDict.Count == MaxCacheSize;
        }
    }
}
