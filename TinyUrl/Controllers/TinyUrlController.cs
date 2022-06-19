using Microsoft.AspNetCore.Mvc;
using TinyUrl.Exceptions;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Uri))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Uri>> Get(Uri tinyUrl)
        {
            try
            {
                Uri originalUrl = await _tinyUrlService.GetOriginalUrl(tinyUrl);
                return Ok(originalUrl);
            }
            catch (InvalidUrlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("CreateTinyUrl")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Uri))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Uri>> CreateAsync([FromBody] Uri url)
        {
            try
            {
                Uri tinyUrl = await _tinyUrlService.CreateTinyUrl(url);
                return tinyUrl is null ? Ok(tinyUrl) : NoContent();
            }
            catch(InvalidUrlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
