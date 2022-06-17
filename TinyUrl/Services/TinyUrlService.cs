using TinyUrl.Dal;
using TinyUrl.Models;
using TinyUrl.Validators;

namespace TinyUrl.Services
{
    public class TinyUrlService : ITinyUrlService
    {
        private readonly ITinyUrlValidator _urlValidator;
        private readonly ITinyUrlGeneretor _tinyUrlGenerator;
        private readonly ITinyUrlDal _tinyUrlDal;

        public TinyUrlService(ITinyUrlValidator urlValidator, ITinyUrlGeneretor tinyUrlGeneretor, ITinyUrlDal tinyUrlDal)
        {
            _urlValidator = urlValidator ?? throw new ArgumentNullException(nameof(urlValidator));
            _tinyUrlGenerator = tinyUrlGeneretor ?? throw new ArgumentNullException(nameof(tinyUrlGeneretor));
            _tinyUrlDal = tinyUrlDal ?? throw new ArgumentNullException(nameof(tinyUrlDal));
        }
        public async Task<Uri> CreateTinyUrl(Uri url)
        {
            _urlValidator.ValidateUrl(url);
            UrlModel? result = await _tinyUrlDal.InsertTinyUrl(url);

            var tinyUrl = $"{url.Scheme}://{url.Host}/{result._id}";
            return new Uri(tinyUrl);
        }

        public async Task<Uri> GetOriginal(Uri url)
        {
            _urlValidator.ValidateUrl(url);
            UrlModel? res =  await _tinyUrlDal.GetOriginal(url);
            return res?.OriginalUrl;
        }
    }
}
