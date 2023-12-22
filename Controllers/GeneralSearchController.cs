using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoaringAPI.Models;
using RoaringAPI.Search;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GeneralSearchController : ControllerBase
{
    private readonly ILogger<GeneralSearchController> _logger;
    private readonly GeneralSearchService _searchService;

    public GeneralSearchController(ILogger<GeneralSearchController> logger, GeneralSearchService searchService)
    {
        _logger = logger;
        _searchService = searchService;
    }

    [HttpGet("{searchTerm}")]
    public async Task<ActionResult<SearchResults>> GeneralSearch(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term is required.");
        }

        var results = await _searchService.GeneralSearch(searchTerm);

        _logger.LogInformation($"Search results: {JsonConvert.SerializeObject(results)}");

        return Ok(results);
    }
}
