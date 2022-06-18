namespace TinyUrl.Validators
{
    public class TinyUrlValidator : ITinyUrlValidator
    {
        public void ValidateUrl(Uri url)
        {
            if (!IsValidUrl(url.OriginalString))
            {
                throw new InvalidOperationException("Given URL is not valid");
            }
        }

        private bool IsValidUrl(string urlOriginalString)
        {
            return Uri.TryCreate(urlOriginalString, UriKind.Absolute, out Uri? uriResult)
                && (uriResult?.Scheme == Uri.UriSchemeHttp || uriResult?.Scheme == Uri.UriSchemeHttps);
        }
    }
}
