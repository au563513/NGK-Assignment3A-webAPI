using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using VejrstationAPI.Data;
using VejrstationAPI.Hubs;
using VejrstationAPI.Models;

namespace VejrstationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VejrobservationerController : ControllerBase
    {
        private readonly VejrstationAPIContext _context;
        private readonly IHubContext<VejrHub> _vejrHubContext;

        public VejrobservationerController(IHubContext<VejrHub> vejrHub, VejrstationAPIContext context)
        {
            _vejrHubContext = vejrHub;
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
                .AsNoTracking()
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

        // GET: api/Vejrobservationer/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Vejrobservation>> GetVejrobservationById(int id)
        {
            var vejrobservation = await _context.Vejrobservationer//.AsNoTracking()
                .Include(v => v.Sted)
                .Where(v => v.VejrobservationId == id)
                .FirstAsync();
            
            if (vejrobservation == null)
            {
                return NotFound();
            }

            vejrobservation.Sted.Vejrobservationer = null;
            vejrobservation.StedNavn = null;

            return vejrobservation;
        }

        // GET: api/Vejrobservationer/last
        [HttpGet("last")]
        public async Task<ActionResult<List<Vejrobservation>>> GetSidsteVejrobservationer()
        {
            var list = await _context.Vejrobservationer
                .AsNoTracking()
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

        // GET: api/Vejrobservationer/2020-01-01T00:00:00/2020-12-12T00:00:00
        [HttpGet("{start:DateTime}/{end:DateTime}")]
        public async Task<ActionResult<List<Vejrobservation>>> GetVejrobservationer(DateTime start, DateTime end)
        {
            if (end <= start)
            {
                return NotFound();
            }

            var list = await _context.Vejrobservationer
                .AsNoTracking()
                .Where(v=>v.Tidspunkt >= start && v.Tidspunkt <= end)
                .OrderByDescending(v=>v.Tidspunkt)
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
                vejrobservation.Sted.Vejrobservationer = null;
                _context.Steder.Add(vejrobservation.Sted);
                await _context.SaveChangesAsync();
            }

            if (vejrobservation.StedNavn == null)
            {
                vejrobservation.StedNavn = vejrobservation.Sted.Navn;
            }

            vejrobservation.Sted = null; //Ikke slet virker ikke uden
            _context.Vejrobservationer.Add(vejrobservation);
            await _context.SaveChangesAsync();
            vejrobservation.Sted = null; //Ikke slet virker ikke uden

            await _vejrHubContext.Clients.All.SendAsync("updateObservation", vejrobservation.VejrobservationId);

            return CreatedAtAction("GetVejrobservationById", new {id = vejrobservation.VejrobservationId}, vejrobservation);
        }
    }
}
