using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Controllers;
using RoaringAPI.Model;
using System.Collections.Generic;

namespace RoaringAPI.Dashboard
{
    public class DashboardResults
    {
        private readonly RoaringDbcontext _context;
        public DashboardResults(RoaringDbcontext context, ILogger<CompanyDataController> logger)
        {
            _context = context;
        }
        public async Task<ActionResult<CompanyRelatedData>> CombineDataDashboard(string roaringCompanyId)
        {
            var lowerCaseId = roaringCompanyId.ToLower();
            var companies = await _context.Companies
                                          .Where(c => c.RoaringCompanyId.ToLower() == lowerCaseId)
                                          .ToListAsync();

            List<int> companyIds = GetCompanyId(companies);

            return new CompanyRelatedData
            {
                Companies = companies,
                Addresses = await GetAddresses(companyIds),
                CompanyEmployees = await GetCompanyEmployees(companyIds),
                FinancialRecords = await GetFinancialRecords(companyIds),
                CompanyRatings = await GetCompanyRatings(companies),
                CompanyStructures = await GetCompanyStructure(companyIds),
            };
        }

        private async Task<List<CompanyStructure>> GetCompanyStructure(List<int> companyIds)
        {
            return await _context.CompanyStructures
            .Where(s => companyIds.Contains(s.CompanyId))
            .ToListAsync();
        }

        private async Task<List<CompanyRating>> GetCompanyRatings(List<Company> companies)
        {
            var companyRatingIds = GetCompanyRatingIds(companies);
            return await _context.CompanyRatings
            .Where(cr => companyRatingIds.Contains(cr.CompanyRatingId))
            .ToListAsync();
        }

        private static List<int> GetCompanyRatingIds(List<Company> companies)
        {
            return companies
             .Where(c => c.CompanyRatingId.HasValue)
            .Select(c => c.CompanyRatingId.Value)
             .Distinct()
             .ToList();
        }

        private async Task<List<CompanyEmployee>> GetCompanyEmployees(List<int> companyIds)
        {
            return await _context.CompanyEmployees
            .Where(e => companyIds.Contains(e.CompanyId))
            .ToListAsync();
        }

        private async Task<List<FinancialRecord>> GetFinancialRecords(List<int> companyIds)
        {
            return await _context.FinancialRecords
            .Where(fr => companyIds.Contains(fr.CompanyId))
            .ToListAsync();
        }

        private async Task<List<Address>> GetAddresses(List<int> companyIds)
        {
            return await _context.Addresses
            .Where(a => companyIds.Contains(a.CompanyId))
            .ToListAsync();
        }

        private static List<int> GetCompanyId(List<Company> companies)
        {
            return companies.Select(c => c.CompanyId).ToList();
        }
    }
}
