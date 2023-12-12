using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Models; 
using RoaringAPI.Search;

[ApiController]
[Route("api/[controller]")]
public class FilteredSearchController : ControllerBase
{
    private readonly ILogger<FilteredSearchController> _logger;
    private readonly FilteredSearchService _searchService;

    public FilteredSearchController(ILogger<FilteredSearchController> logger, FilteredSearchService searchService)
    {
        _logger = logger;
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<ActionResult<SearchResults>> FilteredSearch(string? companyName = null, string? roaringCompanyId = null, DateTime? startDate = null, DateTime? endDate = null, int? minRating = null, int? maxRating = null)
    {
        var results = await _searchService.FilteredSearch(companyName, roaringCompanyId, startDate, endDate, minRating, maxRating);
        return Ok(results);
    }
}



