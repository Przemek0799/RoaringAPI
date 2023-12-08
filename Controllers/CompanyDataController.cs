using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//brings most of the data to dashboard
namespace RoaringAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyDataController : ControllerBase
    {
        private readonly RoaringDbcontext _context;
        private readonly ILogger<CompanyDataController> _logger;

        public CompanyDataController(RoaringDbcontext context, ILogger<CompanyDataController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{roaringCompanyId}")]
        public async Task<ActionResult<CompanyRelatedData>> GetCompanyData(string roaringCompanyId)
        {
            _logger.LogInformation($"Received request for company data with RoaringCompanyId: {roaringCompanyId}");

            try
            {
                var lowerCaseId = roaringCompanyId.ToLower();
                var companies = await _context.Companies
                                              .Where(c => c.RoaringCompanyId.ToLower() == lowerCaseId)
                                              .ToListAsync();

                _logger.LogInformation($"Found {companies.Count} companies matching RoaringCompanyId: {roaringCompanyId}");

                var companyIds = companies.Select(c => c.CompanyId).ToList();

                var addresses = await _context.Addresses
                                              .Where(a => companyIds.Contains(a.CompanyId))
                                              .ToListAsync();
                var employees = await _context.CompanyEmployees
                                              .Where(e => companyIds.Contains(e.CompanyId))
                                              .ToListAsync();
                var financialRecords = await _context.FinancialRecords
                                               .Where(fr => companyIds.Contains(fr.CompanyId))
                                                .ToListAsync();
                var companyRatingIds = companies
                 .Where(c => c.CompanyRatingId.HasValue)
                 .Select(c => c.CompanyRatingId.Value)
                 .Distinct()
                 .ToList();

                var companyRatings = await _context.CompanyRatings
                    .Where(cr => companyRatingIds.Contains(cr.CompanyRatingId))
                    .ToListAsync();
                var companyStructures = await _context.CompanyStructures
                                                      .Where(s => companyIds.Contains(s.CompanyId))
                                                      .ToListAsync();

                _logger.LogInformation($"Found {addresses.Count} addresses, {employees.Count} employees, and {financialRecords.Count},  financial records related to the company.");

                return new CompanyRelatedData
                {
                    Companies = companies,
                    Addresses = addresses,
                    CompanyEmployees = employees,
                    FinancialRecords = financialRecords,
                    CompanyRatings = companyRatings,
                    CompanyStructures = companyStructures,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching data for RoaringCompanyId: {roaringCompanyId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class CompanyRelatedData
    {
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<CompanyEmployee> CompanyEmployees { get; set; }
        public IEnumerable<FinancialRecord> FinancialRecords { get; set; }
        public IEnumerable<CompanyRating> CompanyRatings { get; set; }
        public IEnumerable<CompanyStructure> CompanyStructures { get; set; }


    }
}
