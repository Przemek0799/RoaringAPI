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


//This controller is for general search in navbar and here you can decide what the searchbar will find in DB
public class SearchController : ControllerBase
{
    private readonly RoaringDbcontext _context;
    private readonly ILogger<SearchController> _logger;

    public SearchController(RoaringDbcontext context, ILogger<SearchController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{searchTerm}")]
    public async Task<ActionResult<SearchResults>> GeneralSearch(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term is required.");
        }

        var lowerCaseTerm = searchTerm.ToLower();
        var results = new SearchResults
        {
            Companies = await _context.Companies
                                      .Where(c => EF.Functions.Like(c.CompanyName.ToLower(), $"%{lowerCaseTerm}%") ||
                                                  c.RoaringCompanyId.ToLower().Contains(lowerCaseTerm))
                                      .ToListAsync(),
            CompanyEmployees = await _context.CompanyEmployees
                                             .Where(ce => EF.Functions.Like(ce.TopDirectorName.ToLower(), $"%{lowerCaseTerm}%"))
                                             .ToListAsync(),
            // Add similar OR conditions for other entities if needed
            // Example:
            // CompanyRatings = _context.CompanyRatings.Where(cr => cr.SomeProperty.Contains(searchTerm) || ...).ToList(),
            // CompanyStructures = _context.CompanyStructures.Where(cs => cs.SomeProperty.Contains(searchTerm) || ...).ToList(),
            // FinancialRecords = _context.FinancialRecords.Where(fr => fr.SomeProperty.Contains(searchTerm) || ...).ToList(), 
            // Addresses = _context.Addresses.Where(a => a.SomeProperty.Contains(searchTerm) || ...).ToList(),
        };

        _logger.LogInformation($"Search results: {JsonConvert.SerializeObject(results)}");

        return Ok(results);
    }
}
