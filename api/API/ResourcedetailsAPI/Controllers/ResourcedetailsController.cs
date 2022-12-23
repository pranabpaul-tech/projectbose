using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourcedetailsAPI.DBService;
using ResourcedetailsAPI.Models;

namespace ResourcedetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcedetailsController : ControllerBase
    {
        private readonly mydbContext _context;

        public ResourcedetailsController(mydbContext context)
        {
            _context = context;
        }

        // GET: api/Resourcedetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resourcedetail>>> GetResourcedetails()
        {
            return await _context.Resourcedetails.ToListAsync();
        }

        // GET: api/Resourcedetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resourcedetail>> GetResourcedetail(int id)
        {
            var resourcedetail = await _context.Resourcedetails.FindAsync(id);

            if (resourcedetail == null)
            {
                return NotFound();
            }

            return resourcedetail;
        }

        // PUT: api/Resourcedetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResourcedetail(int id, Resourcedetail resourcedetail)
        {
            if (id != resourcedetail.Resourcedetailid)
            {
                return BadRequest();
            }

            _context.Entry(resourcedetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourcedetailExists(id))
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

        // POST: api/Resourcedetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Resourcedetail>> PostResourcedetail(Resourcedetail resourcedetail)
        {
            _context.Resourcedetails.Add(resourcedetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResourcedetail", new { id = resourcedetail.Resourcedetailid }, resourcedetail);
        }

        // DELETE: api/Resourcedetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResourcedetail(int id)
        {
            var resourcedetail = await _context.Resourcedetails.FindAsync(id);
            if (resourcedetail == null)
            {
                return NotFound();
            }

            _context.Resourcedetails.Remove(resourcedetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResourcedetailExists(int id)
        {
            return _context.Resourcedetails.Any(e => e.Resourcedetailid == id);
        }
    }
}
