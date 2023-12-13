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
        private readonly IRoaringApiService _roaringApiService;
        private readonly ICompanyMapperService _companyMapperService;
        private readonly IAddressMapperService _addressMapperService;
        private readonly ICompanyEmployeeMapperService _companyEmployeeMapperService;

        public CompanySearchController(
            IRoaringApiService roaringApiService,
            ICompanyMapperService companyMapperService,
            IAddressMapperService addressMapperService,
            ICompanyEmployeeMapperService companyEmployeeMapperService)
        {
            _roaringApiService = roaringApiService;
            _companyMapperService = companyMapperService;
            _addressMapperService = addressMapperService;
            _companyEmployeeMapperService = companyEmployeeMapperService;
        }

        [HttpGet("searchByFreeText/{freeText}")]
        public async Task<IActionResult> SearchByFreeText(string freeText)
        {
            var result = await _roaringApiService.FetchCompanyByFreeTextAsync(freeText);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
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

                    // Assuming that the address and employee data are part of the same record
                    var address = await _addressMapperService.HandleAddressAsync(record, company.CompanyId);
                    var employee = await _companyEmployeeMapperService.HandleCompanyEmployeeAsync(record, company.CompanyId);

                    return Ok(new { CompanyId = company?.CompanyId, AddressId = address?.AddressId, EmployeeId = employee?.EmployeeInCompanyId });
                }
                return NotFound("Company data not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}

