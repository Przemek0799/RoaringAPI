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
    public class CompanyRatingsController : ControllerBase
    {
        private readonly RoaringDbcontext _context;

        public CompanyRatingsController(RoaringDbcontext context)
        {
            _context = context;
        }

        // GET: api/CompanyRatings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyRating>>> GetCompanyRatings()
        {
          if (_context.CompanyRatings == null)
          {
              return NotFound();
          }
            return await _context.CompanyRatings.ToListAsync();
        }

        // GET: api/CompanyRatings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyRating>> GetCompanyRating(int id)
        {
          if (_context.CompanyRatings == null)
          {
              return NotFound();
          }
            var companyRating = await _context.CompanyRatings.FindAsync(id);

            if (companyRating == null)
            {
                return NotFound();
            }

            return companyRating;
        }

        // PUT: api/CompanyRatings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyRating(int id, CompanyRating companyRating)
        {
            if (id != companyRating.CompanyRatingId)
            {
                return BadRequest();
            }

            _context.Entry(companyRating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyRatingExists(id))
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

        // POST: api/CompanyRatings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyRating>> PostCompanyRating(CompanyRating companyRating)
        {
          if (_context.CompanyRatings == null)
          {
              return Problem("Entity set 'RoaringDbcontext.CompanyRatings'  is null.");
          }
            _context.CompanyRatings.Add(companyRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompanyRating", new { id = companyRating.CompanyRatingId }, companyRating);
        }

        // DELETE: api/CompanyRatings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyRating(int id)
        {
            if (_context.CompanyRatings == null)
            {
                return NotFound();
            }
            var companyRating = await _context.CompanyRatings.FindAsync(id);
            if (companyRating == null)
            {
                return NotFound();
            }

            _context.CompanyRatings.Remove(companyRating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyRatingExists(int id)
        {
            return (_context.CompanyRatings?.Any(e => e.CompanyRatingId == id)).GetValueOrDefault();
        }
    }
}
