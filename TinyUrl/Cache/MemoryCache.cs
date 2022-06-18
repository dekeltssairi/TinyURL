using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TinyUrl.Models;

namespace TinyUrl.Cache
{
    public class UrlMemoryCache : IUrlMemoryCache
    {
        public int MaxCacheSize => 2;

        public int CacheIndex { get;private set; } // remove

        public Dictionary<TwoKeyDictionary, UrlModel> KeyValuePairs  { get; private set; }


        public UrlMemoryCache()
        {
            KeyValuePairs = new Dictionary<TwoKeyDictionary, UrlModel>();
        }

        public UrlModel GetOrCreate(Uri key, UrlModel urlModel)
        {
            TwoKeyDictionary keyDictionary = new TwoKeyDictionary()
            {
                OriginalUrl = key
            };

            if (!KeyValuePairs.ContainsKey(keyDictionary))
            {
                KeyValuePairs[keyDictionary] = urlModel;
                CacheIndex = (CacheIndex + 1) % MaxCacheSize; // remove
            }

            return KeyValuePairs[keyDictionary];
        }

        public UrlModel Get(Uri key)
        {
            TwoKeyDictionary keyDictionary = new TwoKeyDictionary()
            {
                TinyUrl = key
            };

            return KeyValuePairs.ContainsKey(keyDictionary) ? KeyValuePairs[keyDictionary] : default;
        }

        public void Clear()
        {
            KeyValuePairs.Clear();
        }


    }

    public class TwoKeyDictionary
    {
        public Uri TinyUrl { get; set; }
        public Uri OriginalUrl { get; set; }


        public override bool Equals(object? obj)
        {
            return obj.Equals(TinyUrl) || obj.Equals(OriginalUrl);
        }

        public static bool operator ==(TwoKeyDictionary lhs, TwoKeyDictionary rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.TinyUrl.Equals(rhs.TinyUrl) || lhs.OriginalUrl.Equals(rhs.OriginalUrl);
        }

        public static bool operator !=(TwoKeyDictionary lhs, TwoKeyDictionary rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.TinyUrl != rhs.TinyUrl && lhs.OriginalUrl != rhs.OriginalUrl;
        }
    }
}
