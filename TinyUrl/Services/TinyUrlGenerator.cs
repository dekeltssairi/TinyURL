using Murmur;
using System.Text;

namespace TinyUrl.Services
{
    public class TinyUrlGenerator : ITinyUrlGeneretor
    {
        public Uri GenreateTinyUrl(Uri url)
        {
            Murmur32 murmur = MurmurHash.Create32();
            byte[] bytes = Encoding.UTF8.GetBytes(url.LocalPath);
            byte[] hash = murmur.ComputeHash(bytes);

            string tinyUrl =  $"{url.Scheme}//{url.Host}/{BitConverter.ToString(hash)}";
            return new Uri(tinyUrl);
        }
    }
}
