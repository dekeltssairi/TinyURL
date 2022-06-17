namespace TinyUrl.Validators
{
    public class TinyUrlValidator : ITinyUrlValidator
    {
        public void ValidateUrl(Uri url)
        {
            if (!(url.Scheme == Uri.UriSchemeHttp) && !(url.Scheme == Uri.UriSchemeHttps)) // TO DO - Use exceptionHandler
            {
                throw new InvalidOperationException("Given URL is not valid");
            }
        }
    }
}
