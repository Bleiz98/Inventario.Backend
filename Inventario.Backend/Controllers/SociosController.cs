using Inventario.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SociosController : ControllerBase
    {
        private readonly DataContext _context;

        public SociosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Socio>>> GetSocios()
        {
            return await _context.Socios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Socio>> GetSocio(int id)
        {
            var socio = await _context.Socios.FindAsync(id);
            if (socio == null)
                return NotFound();
            return socio;
        }

        //[HttpPost]
        //public async Task<ActionResult<Socio>> PostSocio(Socio socio)
        //{
        //    _context.Socios.Add(socio);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetSocio), new { id = socio.Id }, socio);
        //}
        [HttpPost]
        public async Task<ActionResult<Socio>> PostSocio(Socio socio)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Devuelve errores al frontend

            _context.Socios.Add(socio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSocio), new { id = socio.Id }, socio);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutSocio(int id, Socio socio)
        {
            if (id != socio.Id) return BadRequest();
            _context.Entry(socio).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSocio(int id)
        {
            var socio = await _context.Socios.FindAsync(id);
            if (socio == null) return NotFound();
            _context.Socios.Remove(socio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

