using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;
using RoaringAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RoaringAPI.Search
{
    public class GeneralSearchService
    {
        private readonly RoaringDbcontext _context;

        public GeneralSearchService(RoaringDbcontext context)
        {
            _context = context;
        }

        public async Task<SearchResults> GeneralSearch(string searchTerm)
        {
            var lowerCaseTerm = searchTerm.ToLower();
            var companyQuery = ApplyCompanyNameFilter(lowerCaseTerm);
            var employeeQuery = ApplyEmployeeNameFilter(lowerCaseTerm);

            return new SearchResults
            {
                Companies = await companyQuery.ToListAsync(),
                CompanyEmployees = await employeeQuery.ToListAsync(),
            };
        }

        private IQueryable<Company> ApplyCompanyNameFilter(string searchTerm)
        {
            return _context.Companies.Where(c => c.CompanyName.ToLower().Contains(searchTerm) || c.RoaringCompanyId.ToLower().Contains(searchTerm));
        }

        private IQueryable<CompanyEmployee> ApplyEmployeeNameFilter(string searchTerm)
        {
            return _context.CompanyEmployees.Where(ce => ce.TopDirectorName.ToLower().Contains(searchTerm));
        }
    }
}
