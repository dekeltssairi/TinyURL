using TinyUrl.Models;

namespace TinyUrl.Cache
{
    public interface IUrlMemoryCache
    {
        public int? MaxCacheSize { get;}

        public void InsertIfNotExist(UrlModel urlModel);
        public UrlModel Get(Uri key);

        public Dictionary<Uri, UrlModel> OrginalUrlToModelDict { get; }
        public Dictionary<Uri, UrlModel> TinyUrlToModelDict { get; }

        public void Clear();
        bool IsFull();
    }
}
