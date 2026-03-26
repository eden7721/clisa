using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Models;
using ServicioTecnicoAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServicioTecnicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposServicioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposServicioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoServicio>>> GetAll()
        {
            var tiposServicio = await _context.TiposServicio.ToListAsync();

            return Ok(tiposServicio);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoServicio>> GetById([FromRoute] int id)
        {
            var tipoServicio = await _context.TiposServicio.FindAsync(id);
            if (tipoServicio == null) return NotFound();

            return Ok(tipoServicio);
        }

        [HttpPost]
        public async Task<ActionResult<TipoServicio>> Create([FromBody] TipoServicio tipoServicio)
        {
            _context.TiposServicio.Add(tipoServicio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = tipoServicio.Id }, tipoServicio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TipoServicio>> Update([FromRoute] int id, [FromBody] TipoServicio tipoServicio)
        {
            var tipoServicioMod = await _context.TiposServicio.FindAsync(id);

            if (tipoServicioMod == null) return NotFound();

            tipoServicioMod.Servicio = tipoServicio.Servicio;

            await _context.SaveChangesAsync();

            return Ok(tipoServicioMod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var tipoServicio = await _context.TiposServicio.FindAsync(id);
            if (tipoServicio == null) return NotFound();
            _context.TiposServicio.Remove(tipoServicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
