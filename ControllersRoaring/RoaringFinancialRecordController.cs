﻿using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Service;
using RoaringAPI.Model;
using RoaringAPI.Interface;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
[Authorize]
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
        _logger = logger;
        _imapperService = financialRecordMapper;
    }

    [HttpPost("fetchandsavefinancialrecords/{companyId}")]
    public async Task<IActionResult> FetchAndSaveFinancialRecords(string companyId)
    {
        try
        {
            var financialData = await _roaringApiService.FetchCompanyFinancialRecordAsync(companyId);

            if (financialData == null || financialData.Records == null || !financialData.Records.Any())
            {
                _logger.LogInformation($"No financial data found for company ID: {companyId}");
                return NotFound($"No financial data found for company ID: {companyId}");
            }

            var company = await _imapperService.HandleCompanyAsync(companyId);
            foreach (var record in financialData.Records)
            {
                await _imapperService.HandleFinancialRecordAsync(record, company.CompanyId);
            }

            _logger.LogInformation($"Successfully fetched and saved {financialData.Records.Count} records for company ID: {companyId}");
            return Ok(new { Message = $"Successfully fetched and saved {financialData.Records.Count} records for company ID: {companyId}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while fetching and saving financial records for company ID: {companyId}");
            return StatusCode(500, "An internal error occurred");
        }
    }


}

