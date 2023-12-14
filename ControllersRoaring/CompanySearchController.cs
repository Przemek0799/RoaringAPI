using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Interface;
using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;
using System.Threading.Tasks;

namespace RoaringAPI.ControllersRoaring
{
    [ApiController]
    [Route("[controller]")]
    public class CompanySearchController : ControllerBase
    {
        private readonly ILogger<CompanySearchController> _logger;
        private readonly IRoaringApiService _roaringApiService;
        private readonly ICompanyMapperService _companyMapperService;
        private readonly IAddressMapperService _addressMapperService;
        private readonly ICompanyEmployeeMapperService _companyEmployeeMapperService;

        public CompanySearchController(
            ILogger<CompanySearchController> logger,
            IRoaringApiService roaringApiService,
            ICompanyMapperService companyMapperService,
            IAddressMapperService addressMapperService,
            ICompanyEmployeeMapperService companyEmployeeMapperService)
        {
            _logger = logger;
            _roaringApiService = roaringApiService;
            _companyMapperService = companyMapperService;
            _addressMapperService = addressMapperService;
            _companyEmployeeMapperService = companyEmployeeMapperService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] Dictionary<string, string> searchParams)
        {
            var result = await _roaringApiService.FetchCompanySearchAsync(searchParams);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("No matching data found.");
        }


        [HttpPost("saveCompany/{companyId}")]
        public async Task<IActionResult> SaveCompany(string companyId)
        {
            try
            {
                var companyData = await _roaringApiService.FetchDataAsync(companyId);

                if (companyData != null && companyData.Records.Any())
                {
                    var record = companyData.Records.First();
                    var company = await _companyMapperService.HandleCompanyAsync(record);

                    var address = await _addressMapperService.HandleAddressAsync(record, company.CompanyId);
                    var employee = await _companyEmployeeMapperService.HandleCompanyEmployeeAsync(record, company.CompanyId);

                    return Ok(new { CompanyId = company?.CompanyId, RoaringCompanyId = record?.CompanyId, AddressId = address?.AddressId, EmployeeId = employee?.EmployeeInCompanyId });
                }
                return NotFound("Company data not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving company data");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}

