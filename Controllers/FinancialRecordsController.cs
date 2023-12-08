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
    public class FinancialRecordsController : ControllerBase
    {
        private readonly RoaringDbcontext _context;

        public FinancialRecordsController(RoaringDbcontext context)
        {
            _context = context;
        }

        // GET: api/FinancialRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialRecord>>> GetFinancialRecords()
        {
          if (_context.FinancialRecords == null)
          {
              return NotFound();
          }
            return await _context.FinancialRecords.ToListAsync();
        }

        // GET: api/FinancialRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialRecord>> GetFinancialRecord(int id)
        {
          if (_context.FinancialRecords == null)
          {
              return NotFound();
          }
            var financialRecord = await _context.FinancialRecords.FindAsync(id);

            if (financialRecord == null)
            {
                return NotFound();
            }

            return financialRecord;
        }

        // PUT: api/FinancialRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinancialRecord(int id, FinancialRecord financialRecord)
        {
            if (id != financialRecord.FinancialRecordID)
            {
                return BadRequest();
            }

            _context.Entry(financialRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialRecordExists(id))
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

        // POST: api/FinancialRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FinancialRecord>> PostFinancialRecord(FinancialRecord financialRecord)
        {
          if (_context.FinancialRecords == null)
          {
              return Problem("Entity set 'Dbcontext.FinancialRecords'  is null.");
          }
            _context.FinancialRecords.Add(financialRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinancialRecord", new { id = financialRecord.FinancialRecordID }, financialRecord);
        }

        // DELETE: api/FinancialRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialRecord(int id)
        {
            if (_context.FinancialRecords == null)
            {
                return NotFound();
            }
            var financialRecord = await _context.FinancialRecords.FindAsync(id);
            if (financialRecord == null)
            {
                return NotFound();
            }

            _context.FinancialRecords.Remove(financialRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinancialRecordExists(int id)
        {
            return (_context.FinancialRecords?.Any(e => e.FinancialRecordID == id)).GetValueOrDefault();
        }
    }
}
