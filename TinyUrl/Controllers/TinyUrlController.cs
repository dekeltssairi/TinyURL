using Microsoft.AspNetCore.Mvc;
using TinyUrl.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TinyUrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinyUrlController : ControllerBase
    {
        private readonly ITinyUrlService _tinyUrlService;

        public TinyUrlController(ITinyUrlService tinyUrlService)
        {
            _tinyUrlService = tinyUrlService ?? throw new ArgumentNullException(nameof(tinyUrlService));
        }

        [HttpGet]
        public async Task<Uri> Get(Uri url)
        {
            return await _tinyUrlService.GetOriginal(url);
        }

        [HttpPost]
        public async Task<Uri> Post([FromBody] Uri url)
        {
            return await _tinyUrlService.CreateTinyUrl(url);
        }
    }
}
