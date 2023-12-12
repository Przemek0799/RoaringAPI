using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;
using RoaringAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoaringAPI.Search
{
    public class FilteredSearchService
    {
        private readonly RoaringDbcontext _context;

        public FilteredSearchService(RoaringDbcontext context)
        {
            _context = context;
        }

        public async Task<SearchResults> FilteredSearch(string? companyName, string? roaringCompanyId, DateTime? startDate, DateTime? endDate, int? minRating, int? maxRating)
        {
            IQueryable<Company> query = _context.Companies;

            // Apply filters
            query = ApplyCompanyNameFilter(query, companyName);
            query = ApplyRoaringCompanyIdFilter(query, roaringCompanyId);
            query = ApplyDateFilters(query, startDate, endDate);
            query = ApplyRatingFilters(query, minRating, maxRating);

            return new SearchResults
            {
                Companies = await query.ToListAsync()
            };
        }

        private IQueryable<Company> ApplyCompanyNameFilter(IQueryable<Company> query, string? companyName)
        {
            if (!string.IsNullOrWhiteSpace(companyName))
            {
                var lowerCaseCompanyName = companyName.ToLower();
                query = query.Where(c => c.CompanyName.ToLower().Contains(lowerCaseCompanyName));
            }
            return query;
        }
        private IQueryable<Company> ApplyRoaringCompanyIdFilter(IQueryable<Company> query, string? roaringCompanyId)
        {
            if (!string.IsNullOrWhiteSpace(roaringCompanyId))
            {
                query = query.Where(c => c.RoaringCompanyId.Contains(roaringCompanyId));
            }
            return query;
        }

        private IQueryable<Company> ApplyDateFilters(IQueryable<Company> query, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue)
            {
                query = query.Where(c => c.CompanyRegistrationDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.CompanyRegistrationDate <= endDate.Value);
            }
            return query;
        }

        private IQueryable<Company> ApplyRatingFilters(IQueryable<Company> query, int? minRating, int? maxRating)
        {
            if (minRating.HasValue || maxRating.HasValue)
            {
                query = query.Where(c => _context.CompanyRatings.Any(r => r.CompanyRatingId == c.CompanyRatingId &&
                (!minRating.HasValue || r.Rating >= minRating) &&
                (!maxRating.HasValue || r.Rating <= maxRating)));
            }
            return query;
        }

    }
}
