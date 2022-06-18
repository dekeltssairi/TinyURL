using Murmur;
using System.Text;

namespace TinyUrl.Services
{
    public class TinyUrlGenerator : ITinyUrlGeneretor
    {
        public Uri GenreateTinyUrl(Uri url)
        {
            string tinyUrl =  $"{url.Scheme}://{url.Host}/{Guid.NewGuid()}";
            return new Uri(tinyUrl);
        }
    }
}
