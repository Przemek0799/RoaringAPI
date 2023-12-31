﻿using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Model;
using RoaringAPI.Service;
using RoaringAPI.Interface;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FinancialRatingController : ControllerBase
{
    private readonly RoaringApiService _roaringApiService;
    private readonly RoaringDbcontext _dbContext;
    private readonly ILogger<FinancialRatingController> _logger;
    private readonly IFinancialRatingMapperService _imapperService;


    public FinancialRatingController(RoaringApiService roaringApiService, RoaringDbcontext dbContext, ILogger<FinancialRatingController> logger, IFinancialRatingMapperService imapperService) 
    {
        _roaringApiService = roaringApiService ?? throw new ArgumentNullException(nameof(roaringApiService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
        _imapperService = imapperService;
    }

    [HttpPost("fetchandsavecompanyrating/{companyId}")]
    public async Task<IActionResult> FetchAndSaveCompanyRating(string companyId)
    {
        try
        {
            var companyRatingData = await _roaringApiService.FetchCompanyRatingAsync(companyId);

            if (companyRatingData == null)
            {
                return NotFound($"No company rating data found for company ID: {companyId}");
            }

            // Pass both companyId and companyRatingData to the method
            var companyRating = await _imapperService.CreateOrUpdateCompanyRatingAsync(companyId, companyRatingData);
            var company = await _imapperService.HandleCompanyAsync(companyId, companyRating.CompanyRatingId);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in FetchAndSaveCompanyRating.");
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}

