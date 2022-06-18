using Microsoft.AspNetCore.Mvc;
using TinyUrl.Services;

namespace TinyUrl.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TinyUrlController : ControllerBase
    {
        private readonly ITinyUrlService _tinyUrlService;

        public TinyUrlController(ITinyUrlService tinyUrlService)
        {
            _tinyUrlService = tinyUrlService ?? throw new ArgumentNullException(nameof(tinyUrlService));
        }

        [HttpGet("GetTinyUrl")]
        public async Task<Uri> Get(Uri url)
        {
            return await _tinyUrlService.GetOriginalUrl(url);
        }

        [HttpPost("CreateTinyUrl")]
        public async Task<Uri> CreateAsync([FromBody] Uri url)
        {
            return await _tinyUrlService.CreateTinyUrl(url);
        }
    }
}
