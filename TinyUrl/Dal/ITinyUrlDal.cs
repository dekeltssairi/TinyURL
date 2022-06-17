namespace TinyUrl.Dal
{
    public interface ITinyUrlDal
    {
        public Task InsertTinyUrl(Uri originalUrl, Uri tinyUrl);
    }
}
