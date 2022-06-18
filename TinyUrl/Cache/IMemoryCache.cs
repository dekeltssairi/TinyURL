using TinyUrl.Models;

namespace TinyUrl.Cache
{
    public interface IUrlMemoryCache
    {
        public int MaxCacheSize { get;}

        public UrlModel GetOrCreate(Uri key, UrlModel urlModel);
        public UrlModel Get(Uri key);

        public Dictionary<TwoKeyDictionary, UrlModel> KeyValuePairs { get; }

        public void Clear();

        public int CacheIndex { get; }
    }
}
