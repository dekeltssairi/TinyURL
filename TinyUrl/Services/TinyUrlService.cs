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
        public Task<Uri> CreateTinyUrl(Uri url)
        {
            _urlValidator.ValidateUrl(url);
            Uri tinyUrl = _tinyUrlGenerator.GenreateTinyUrl(url);
            return _tinyUrlDal.InsertTinyUrl(url, tinyUrl);
        }

        public Task<Uri> GetOriginalUrl(Uri tinyUrl)
        {
            _urlValidator.ValidateUrl(tinyUrl);
            return  _tinyUrlDal.GetOriginal(tinyUrl);
        }
    }
}
