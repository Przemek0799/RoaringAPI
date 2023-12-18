using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Interface;
using RoaringAPI.Service;

[ApiController]
[Route("[controller]")]
public class RoaringInfoController : ControllerBase
{
    private readonly RoaringApiService _roaringApiService;
    private readonly ICompanyMapperService _companyMapperService;
    private readonly IAddressMapperService _addressMapperService;
    private readonly ICompanyEmployeeMapperService _companyEmployeeMapperService;

    public RoaringInfoController(
          RoaringApiService roaringApiService,
          ICompanyMapperService companyMapperService,
          IAddressMapperService addressMapperService,
          ICompanyEmployeeMapperService companyEmployeeMapperService)
    {
        _roaringApiService = roaringApiService;
        _companyMapperService = companyMapperService;
        _addressMapperService = addressMapperService;
        _companyEmployeeMapperService = companyEmployeeMapperService;

    }

    [HttpPost("FetchAndSaveData/{companyId}")]
    public async Task<IActionResult> FetchAndSaveData(string companyId)
    {
        var roaringData = await _roaringApiService.FetchDataAsync(companyId);
        if (roaringData == null || roaringData.Records == null || !roaringData.Records.Any())
        {
            return NotFound($"No data found for company ID: {companyId}");
        }

        var companyRecord = roaringData.Records.FirstOrDefault();
        if (companyRecord != null)
        {
            var company = await _companyMapperService.HandleCompanyAsync(companyRecord);
            await _addressMapperService.HandleAddressAsync(companyRecord, company.CompanyId);
            await _companyEmployeeMapperService.HandleCompanyEmployeeAsync(companyRecord, company.CompanyId);

            return Ok(new { CompanyId = company?.CompanyId });
        }

        return NotFound($"No valid record found for company ID: {companyId}");
    }
}