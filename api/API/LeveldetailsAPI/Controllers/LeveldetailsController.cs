using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeveldetailsAPI.DBService;
using LeveldetailsAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace LeveldetailsAPI.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeveldetailsController : ControllerBase
    {
        private readonly mydbContext _context;

        public LeveldetailsController(mydbContext context)
        {
            _context = context;
        }

        // GET: api/Leveldetails
        [EnableCors]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leveldetail>>> GetLeveldetails()
        {
            return await _context.Leveldetails.ToListAsync();
        }

        // GET: api/Leveldetails/5
        [EnableCors]
        [HttpGet("{id}")]
        public async Task<ActionResult<Leveldetail>> GetLeveldetail(int id)
        {
            var leveldetail = await _context.Leveldetails.FindAsync(id);

            if (leveldetail == null)
            {
                return NotFound();
            }

            return leveldetail;
        }

        // PUT: api/Leveldetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeveldetail(int id, Leveldetail leveldetail)
        {
            if (id != leveldetail.Leveldetailid)
            {
                return BadRequest();
            }

            _context.Entry(leveldetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeveldetailExists(id))
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

        // POST: api/Leveldetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors]
        [HttpPost]
        public async Task<ActionResult<Leveldetail>> PostLevel(Leveldetail leveldetail)
        {
            _context.Leveldetails.Add(leveldetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeveldetail", new { id = leveldetail.Leveldetailid }, leveldetail);
        }

        // DELETE: api/Leveldetails/5
        [EnableCors]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeveldetail(int id)
        {
            var leveldetail = await _context.Leveldetails.FindAsync(id);
            if (leveldetail == null)
            {
                return NotFound();
            }

            _context.Leveldetails.Remove(leveldetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeveldetailExists(int id)
        {
            return _context.Leveldetails.Any(e => e.Leveldetailid == id);
        }
    }
}
