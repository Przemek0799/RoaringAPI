using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;
//crud för company employees

namespace RoaringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyEmployeesController : ControllerBase
    {
        private readonly RoaringDbcontext _context;

        public CompanyEmployeesController(RoaringDbcontext context)
        {
            _context = context;
        }

        // GET: api/CompanyEmployees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyEmployee>>> GetCompanyEmployees()
        {
          if (_context.CompanyEmployees == null)
          {
              return NotFound();
          }
            return await _context.CompanyEmployees.ToListAsync();
        }

        // GET: api/CompanyEmployees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyEmployee>> GetCompanyEmployee(int id)
        {
          if (_context.CompanyEmployees == null)
          {
              return NotFound();
          }
            var companyEmployee = await _context.CompanyEmployees.FindAsync(id);

            if (companyEmployee == null)
            {
                return NotFound();
            }

            return companyEmployee;
        }

        // PUT: api/CompanyEmployees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyEmployee(int id, CompanyEmployee companyEmployee)
        {
            if (id != companyEmployee.EmployeeInCompanyId)
            {
                return BadRequest();
            }

            _context.Entry(companyEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyEmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CompanyEmployees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyEmployee>> PostCompanyEmployee(CompanyEmployee companyEmployee)
        {
          if (_context.CompanyEmployees == null)
          {
              return Problem("Entity set 'Dbcontext.CompanyEmployees'  is null.");
          }
            _context.CompanyEmployees.Add(companyEmployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompanyEmployee", new { id = companyEmployee.EmployeeInCompanyId }, companyEmployee);
        }

        // DELETE: api/CompanyEmployees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyEmployee(int id)
        {
            if (_context.CompanyEmployees == null)
            {
                return NotFound();
            }
            var companyEmployee = await _context.CompanyEmployees.FindAsync(id);
            if (companyEmployee == null)
            {
                return NotFound();
            }

            _context.CompanyEmployees.Remove(companyEmployee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyEmployeeExists(int id)
        {
            return (_context.CompanyEmployees?.Any(e => e.EmployeeInCompanyId == id)).GetValueOrDefault();
        }
    }
}
