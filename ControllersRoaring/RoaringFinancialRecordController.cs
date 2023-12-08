using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using RoaringAPI.Service;
using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this namespace
using RoaringAPI.Mapping;
using RoaringAPI.Interface;

[ApiController]
[Route("[controller]")]
public class RoaringFinancialRecordController : ControllerBase
{
    private readonly RoaringApiService _roaringApiService;
    private readonly RoaringDbcontext _dbContext;
    private readonly ILogger<RoaringFinancialRecordController> _logger;
    private readonly IFinancialRecordMapperService _imapperService;
    public RoaringFinancialRecordController(RoaringApiService roaringApiService, RoaringDbcontext dbContext, ILogger<RoaringFinancialRecordController> logger, IFinancialRecordMapperService financialRecordMapper)
    {
        _roaringApiService = roaringApiService;
        _dbContext = dbContext;
        _logger = logger; // Initialize logger
        _imapperService = financialRecordMapper;
    }

    [HttpPost("fetchandsavefinancialrecords/{companyId}")]
    public async Task<IActionResult> FetchAndSaveFinancialRecords(string companyId)
    {
        var financialData = await _roaringApiService.FetchCompanyFinancialRecordAsync(companyId);

        if (financialData == null || financialData.Records == null || !financialData.Records.Any())
        {
            return NotFound($"No financial data found for company ID: {companyId}");
        }

        var company = await _imapperService.HandleCompanyAsync(companyId);

        foreach (var record in financialData.Records)
        {
            await _imapperService.HandleFinancialRecordAsync(record, company.CompanyId);
        }

        return Ok();
    }

}

