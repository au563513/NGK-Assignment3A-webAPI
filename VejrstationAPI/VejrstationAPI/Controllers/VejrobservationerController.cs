using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using VejrstationAPI.Data;
using VejrstationAPI.Models;

namespace VejrstationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VejrobservationerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VejrobservationerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vejrobservationer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vejrobservation>>> GetVejrobservationer()
        {
            var list = await _context.Vejrobservationer
                .AsNoTracking()
                .Include(v=>v.Sted)
                .ToListAsync();

            foreach (var vejrobservation in list)
            {
                vejrobservation.Sted.Vejrobservationer = null;
                vejrobservation.StedNavn = null;
            }

            return list;
        }

        // GET: api/Vejrobservationer/2020-01-01T00:00:00
        [HttpGet("{date:DateTime}")]
        public async Task<ActionResult<List<Vejrobservation>>> GetVejrobservation(DateTime date)
        {
            var vejrobservation = await _context.Vejrobservationer
                .Where(v => v.Tidspunkt.Date == date.Date)
                .ToListAsync();

            if (vejrobservation == null)
            {
                return NotFound();  
            }

            return vejrobservation;
        }

        // PUT: api/Vejrobservationer/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVejrobservation(int id, Vejrobservation vejrobservation)
        {
            if (id != vejrobservation.VejrobservationId)
            {
                return BadRequest();
            }

            _context.Entry(vejrobservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VejrobservationExists(id))
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

        // POST: api/Vejrobservationer
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Vejrobservation>> PostVejrobservation(Vejrobservation vejrobservation)
        {
            if (!_context.Steder.Any(s=>s.Navn == vejrobservation.Sted.Navn))
            {
                _context.Steder.Add(vejrobservation.Sted);
                await _context.SaveChangesAsync();
            }

            if (vejrobservation.StedNavn == null)
            {
                vejrobservation.StedNavn = vejrobservation.Sted.Navn;
            }

            vejrobservation.Sted = null;
            _context.Vejrobservationer.Add(vejrobservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVejrobservation", new { date = vejrobservation.Tidspunkt }, vejrobservation);
        }

        // DELETE: api/Vejrobservationer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vejrobservation>> DeleteVejrobservation(int id)
        {
            var vejrobservation = await _context.Vejrobservationer.FindAsync(id);
            if (vejrobservation == null)
            {
                return NotFound();
            }

            _context.Vejrobservationer.Remove(vejrobservation);
            await _context.SaveChangesAsync();

            return vejrobservation;
        }

        private bool VejrobservationExists(int id)
        {
            return _context.Vejrobservationer.Any(e => e.VejrobservationId == id);
        }
    }
}
