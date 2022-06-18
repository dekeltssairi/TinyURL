namespace TinyUrl.Cache
{
    public interface ICache
    {
        public Dictionary<Uri, (LinkedListNode<Uri> node, Uri value, int count)> _cache { get;}
        public Uri Get(Uri uriKey);

        public void Put(Uri uriKey, Uri value);
    }
}
