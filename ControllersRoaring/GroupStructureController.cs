using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Interface;
using RoaringAPI.Service;

[ApiController]
[Route("[controller]")]
public class GroupStructureController : ControllerBase
{
    private readonly RoaringApiService _roaringApiService;
    private readonly IGroupStructureMapperService _imapperService;
    private readonly ILogger<GroupStructureController> _logger;


    public GroupStructureController(RoaringApiService roaringApiService, IGroupStructureMapperService mapperService, ILogger<GroupStructureController> logger)
    {
        _roaringApiService = roaringApiService;
        _imapperService = mapperService;
        _logger = logger;

    }

    [HttpPost("fetchandsavegroupstructure/{companyId}")]
    public async Task<IActionResult> FetchAndSaveGroupStructure(string companyId)
    {
        try
        {
            var groupStructureData = await _roaringApiService.FetchCompanyGroupStructureAsync(companyId);
            if (groupStructureData == null || groupStructureData.GroupCompanies == null)
            {
                return NotFound($"No group structure data found for company ID: {companyId}");
            }

            var mainCompany = await _imapperService.HandleCompanyAsync(groupStructureData.CompanyId, groupStructureData.CompanyName);
            foreach (var groupCompany in groupStructureData.GroupCompanies)
            {
                var company = await _imapperService.HandleCompanyAsync(groupCompany.CompanyId, groupCompany.CompanyName);
                await _imapperService.HandleCompanyStructureAsync(company.CompanyId, groupCompany.MotherCompanyId, groupCompany);
            }

            return Ok();
        }
        catch (Exception ex)
        {
           
            _logger.LogError(ex, "Error occurred in FetchAndSaveGroupStructure.");
            return StatusCode(500, "Internal server error");
        }
    }
}
