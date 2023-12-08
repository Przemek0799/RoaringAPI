using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Model;


namespace RoaringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyStructuresController : ControllerBase
    {
        private readonly RoaringDbcontext _context;

        public CompanyStructuresController(RoaringDbcontext context)
        {
            _context = context;
        }

        // GET: api/CompanyStructures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyStructure>>> GetCompanyStructures()
        {
          if (_context.CompanyStructures == null)
          {
              return NotFound();
          }
            return await _context.CompanyStructures.ToListAsync();
        }

        // GET: api/CompanyStructures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyStructure>> GetCompanyStructure(int id)
        {
          if (_context.CompanyStructures == null)
          {
              return NotFound();
          }
            var companyStructure = await _context.CompanyStructures.FindAsync(id);

            if (companyStructure == null)
            {
                return NotFound();
            }

            return companyStructure;
        }

        // PUT: api/CompanyStructures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyStructure(int id, CompanyStructure companyStructure)
        {
            if (id != companyStructure.CompanyStructureId)
            {
                return BadRequest();
            }

            _context.Entry(companyStructure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyStructureExists(id))
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

        // POST: api/CompanyStructures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyStructure>> PostCompanyStructure(CompanyStructure companyStructure)
        {
          if (_context.CompanyStructures == null)
          {
              return Problem("Entity set 'Dbcontext.CompanyStructures'  is null.");
          }
            _context.CompanyStructures.Add(companyStructure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompanyStructure", new { id = companyStructure.CompanyStructureId }, companyStructure);
        }

        // DELETE: api/CompanyStructures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyStructure(int id)
        {
            if (_context.CompanyStructures == null)
            {
                return NotFound();
            }
            var companyStructure = await _context.CompanyStructures.FindAsync(id);
            if (companyStructure == null)
            {
                return NotFound();
            }

            _context.CompanyStructures.Remove(companyStructure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyStructureExists(int id)
        {
            return (_context.CompanyStructures?.Any(e => e.CompanyStructureId == id)).GetValueOrDefault();
        }
    }
}
