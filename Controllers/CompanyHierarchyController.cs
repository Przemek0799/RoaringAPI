using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


// hämtar data för company strukturen för ett specifikt company id
namespace RoaringAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyHierarchyController : ControllerBase
    {
        private readonly RoaringDbcontext _context;
        private readonly ILogger<CompanyHierarchyController> _logger;

        public CompanyHierarchyController(RoaringDbcontext context, ILogger<CompanyHierarchyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{roaringCompanyId}")]
        public async Task<ActionResult<CompanyHierarchyRelatedData>> GetCompanyData(string roaringCompanyId)
        {
            _logger.LogInformation($"Received request for company data with RoaringCompanyId: {roaringCompanyId}");

            try
            {
                var lowerCaseId = roaringCompanyId.ToLower();
                var companies = await _context.Companies
                                              .Where(c => c.RoaringCompanyId.ToLower() == lowerCaseId)
                                              .ToListAsync();

                _logger.LogInformation($"Found {companies.Count} companies matching RoaringCompanyId: {roaringCompanyId}");
                _logger.LogInformation($"Companies Data: {JsonConvert.SerializeObject(companies)}");


                var companyIds = companies.Select(c => c.CompanyId).ToList();

                var companyStructures = await _context.CompanyStructures
                                       .Include(cs => cs.Company)
                                       .Include(cs => cs.MotherCompany)
                                       .Where(cs => companyIds.Contains(cs.CompanyId))
                                       .ToListAsync();


                _logger.LogInformation($"Found {companyStructures.Count} company structures.");
                if (companyStructures.Any())
                {
                    _logger.LogInformation($"Company Structures Data: {JsonConvert.SerializeObject(companyStructures)}");
                }
                else
                {
                    _logger.LogWarning("No company structures found for the provided CompanyIds.");
                }

                _logger.LogInformation($"Found {companyStructures.Count} companyStructures  financial records related to the company.");
                _logger.LogInformation($"Company Structures Data: {JsonConvert.SerializeObject(companyStructures)}");


                return new CompanyHierarchyRelatedData
                {
                    Companies = companies,
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

    public class CompanyHierarchyRelatedData
    {
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<CompanyEmployee> CompanyEmployees { get; set; }
        public IEnumerable<FinancialRecord> FinancialRecords { get; set; }
        public IEnumerable<CompanyRating> CompanyRatings { get; set; }
        public IEnumerable<CompanyStructure> CompanyStructures { get; set; }


    }
}
