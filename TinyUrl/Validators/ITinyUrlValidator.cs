namespace TinyUrl.Validators
{
    public interface ITinyUrlValidator
    {
        public void ValidateUrl(Uri url);
    }
}
