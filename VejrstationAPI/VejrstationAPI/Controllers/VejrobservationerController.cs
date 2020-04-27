using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly VejrstationAPIContext _context;

        public VejrobservationerController(VejrstationAPIContext context)
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
            var list = await _context.Vejrobservationer
                .Where(v => v.Tidspunkt.Date == date.Date)
                .Include(v=>v.Sted)
                .ToListAsync();

            if (list == null)
            {
                return NotFound();  
            }

            foreach (var vejrobservation in list)
            {
                vejrobservation.Sted.Vejrobservationer = null;
                vejrobservation.StedNavn = null;
            }

            return list;
        }

        // GET: api/Vejrobservationer/last
        [HttpGet("last")]
        public async Task<ActionResult<List<Vejrobservation>>> GetSidsteVejrobservationer()
        {
            var list = await _context.Vejrobservationer
                .OrderByDescending(v=>v.Tidspunkt)
                .Take(3)
                .Include(v=>v.Sted)
                .ToListAsync();

            if (list == null)
            {
                return NotFound();
            }

            foreach (var vejrobservation in list)
            {
                vejrobservation.Sted.Vejrobservationer = null;
                vejrobservation.StedNavn = null;
            }

            return list;
        }

        [HttpGet("{start:DateTime}/{end:DateTime}")]
        public async Task<ActionResult<List<Vejrobservation>>> GetVejrobservationer(DateTime start, DateTime end)
        {
            if (end <= start)
            {
                return NotFound();
            }

            var list = await _context.Vejrobservationer
                .Where(v=>v.Tidspunkt >= start && v.Tidspunkt <= end)
                .Include(v => v.Sted)
                .ToListAsync();

            if (list == null)
            {
                return NotFound();
            }

            foreach (var vejrobservation in list)
            {
                vejrobservation.Sted.Vejrobservationer = null;
                vejrobservation.StedNavn = null;
            }

            return list;
        }

        [Authorize]
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
    }
}
