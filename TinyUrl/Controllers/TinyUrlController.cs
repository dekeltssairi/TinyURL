using Microsoft.AspNetCore.Mvc;
using TinyUrl.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TinyUrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinyUrlController : ControllerBase
    {
        private readonly IBooksService _bookService;
        private readonly ITinyUrlService _tinyUrlService;

        public TinyUrlController(ITinyUrlService tinyUrlService, IBooksService booksService)
        {
            _bookService = booksService ?? throw new ArgumentNullException(nameof(booksService));
            _tinyUrlService = tinyUrlService ?? throw new ArgumentNullException(nameof(tinyUrlService));
        }

        // GET: api/<TinyUrlController>
        [HttpGet]
        public string Get()
        {
            return "value1";
        }

        //// GET api/<TinyUrlController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<TinyUrlController>
        [HttpPost]
        public async Task Post([FromBody] Uri url)
        {
            await _tinyUrlService.CreateTinyUrl(url);
        }
    }
}
