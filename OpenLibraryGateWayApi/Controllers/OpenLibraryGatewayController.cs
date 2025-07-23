using Microsoft.AspNetCore.Mvc;
using OpenLibraryGateWayApi.ApiModels;
using OpenLibraryGateWayApi.Services;

namespace OpenLibraryGateWayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenLibraryGatewayController : ControllerBase
    {
        private readonly ILogger<OpenLibraryGatewayController> _logger;
        private readonly IContactOpenLibraryService _olService;

        public OpenLibraryGatewayController(ILogger<OpenLibraryGatewayController> logger
            , IContactOpenLibraryService openLibrary )
        {
            _logger = logger;
            _olService = openLibrary;
        }

        [HttpGet(Name = "GetSearchResults")]
        [HttpGet]
        [Route("/search/byName")]
        public async Task<ActionResult<OpenLibrarySearchResult>> GetSearchResults([FromQuery] string authorname, [FromQuery] int page = 1)
        {
            try
            {
                var results = await _olService.GetOpenLibrarySearchResults(authorname, page);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "OpenLibrary is temporarily unavailable. Please try again later.");
            }
        
        }
    }
}
