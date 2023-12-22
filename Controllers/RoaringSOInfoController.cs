using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;

namespace RoaringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RoaringSOInfoController : ControllerBase
    {
        private readonly RoaringDbcontext _context;

        public RoaringSOInfoController(RoaringDbcontext context)
        {
            _context = context; 
        }

        [HttpGet]
        public async Task<ActionResult<RoaringSOInfoData>> GetAllData()
        {

            var addresses = await _context.Addresses.ToListAsync();
            var companies = await _context.Companies.ToListAsync();
            var companyEmployees = await _context.CompanyEmployees.ToListAsync();

            return new RoaringSOInfoData
            {
                Addresses = addresses,
                Companies = companies,
                CompanyEmployees = companyEmployees
            };
        }

        [HttpGet("{companyId}")]
        public async Task<ActionResult<RoaringSOInfoData>> GetCompanyData(int companyId)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == companyId);
            if (company == null)
            {
                return NotFound();
            }

            var addresses = await _context.Addresses.Where(a => a.CompanyId == companyId).ToListAsync();
            var companyEmployees = await _context.CompanyEmployees.Where(ce => ce.CompanyId == companyId).ToListAsync();

            return new RoaringSOInfoData
            {
                Addresses = addresses,
                Companies = new List<Company> { company },
                CompanyEmployees = companyEmployees
            };
        }
    }

    // This class should be within the namespace but outside of the controller class
    public class RoaringSOInfoData
    {
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<CompanyEmployee> CompanyEmployees { get; set; }
    }
}
