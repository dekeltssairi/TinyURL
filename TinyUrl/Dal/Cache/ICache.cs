namespace TinyUrl.Cache
{
    public interface ICache
    {
        public Uri Get(Uri uriKey);

        public void Put(Uri uriKey, Uri value);
    }
}
