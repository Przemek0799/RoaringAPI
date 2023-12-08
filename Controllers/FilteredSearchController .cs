using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;
using RoaringAPI.Models; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class FilteredSearchController : ControllerBase
{
    private readonly RoaringDbcontext _context;
    private readonly ILogger<FilteredSearchController> _logger;

    public FilteredSearchController(RoaringDbcontext context, ILogger<FilteredSearchController> logger)
    {
        _context = context;
        _logger = logger;
    }
    [HttpGet]
    public async Task<ActionResult<SearchResults>> FilteredSearch(string? companyName = null, string? roaringCompanyId = null, DateTime? startDate = null, DateTime? endDate = null, int? minRating = null, int? maxRating = null)
    {
        _logger.LogInformation($"Search requested with companyName: {companyName}, roaringCompanyId: {roaringCompanyId}, startDate: {startDate}, endDate: {endDate}, minRating: {minRating}, maxRating: {maxRating}");

        IQueryable<Company> query = _context.Companies;

        if (!string.IsNullOrWhiteSpace(companyName))
        {
            var lowerCaseCompanyName = companyName.ToLower();
            query = query.Where(c => c.CompanyName.ToLower().Contains(lowerCaseCompanyName));
        }

        if (!string.IsNullOrWhiteSpace(roaringCompanyId))
        {
            query = query.Where(c => c.RoaringCompanyId.Contains(roaringCompanyId));
        }

        if (startDate.HasValue)
        {
            query = query.Where(c => c.CompanyRegistrationDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(c => c.CompanyRegistrationDate <= endDate.Value);
        }

        if (minRating.HasValue || maxRating.HasValue)
        {
            query = query.Where(c => _context.CompanyRatings.Any(r => r.CompanyRatingId == c.CompanyRatingId && (!minRating.HasValue || r.Rating >= minRating) && (!maxRating.HasValue || r.Rating <= maxRating)));
        }

        var companies = await query.ToListAsync();

        var results = new SearchResults
        {
            Companies = companies
        };

        return Ok(results);
    }
}


