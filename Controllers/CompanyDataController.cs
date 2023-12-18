using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Model;
using RoaringAPI.Dashboard;

//brings most of the data to dashboard
namespace RoaringAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyDataController : ControllerBase
    {
        private readonly RoaringDbcontext _context;
        private readonly ILogger<CompanyDataController> _logger;
        private readonly DashboardResults _dashboardResults;


        public CompanyDataController(RoaringDbcontext context, ILogger<CompanyDataController> logger, DashboardResults dashboardResults)
        {
            _context = context;
            _logger = logger;
            _dashboardResults = dashboardResults;

        }

        [HttpGet("{roaringCompanyId}")]
        public async Task<ActionResult<CompanyRelatedData>> GetCompanyData(string roaringCompanyId)
        {
            _logger.LogInformation($"Received request for company data with RoaringCompanyId: {roaringCompanyId}");

            try
            {
                return await _dashboardResults.CombineDataDashboard(roaringCompanyId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching data for RoaringCompanyId: {roaringCompanyId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
