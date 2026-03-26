using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Models;
using ServicioTecnicoAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServicioTecnicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosServicioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstadosServicioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EstadoServicio>>> GetAll()
        {
            var estadosServicio = await _context.EstadosServicio.ToListAsync();
            return Ok(estadosServicio);            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoServicio>> GetById([FromRoute] int id)
        {
            var estadoServicio = await _context.EstadosServicio.FindAsync(id);

            if (estadoServicio == null) return NotFound();

            return Ok(estadoServicio);
        }

        [HttpPost]
        public async Task<ActionResult<EstadoServicio>> Create([FromBody] EstadoServicio estadoServicio)
        {
            _context.EstadosServicio.Add(estadoServicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = estadoServicio.Id }, estadoServicio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EstadoServicio>> Update([FromRoute] int id, [FromBody] EstadoServicio estadoServicio)
        {
            var estadoServicioMod = await _context.EstadosServicio.FindAsync(id);
            if (estadoServicioMod == null) return NotFound();

            estadoServicioMod.Estado = estadoServicio.Estado;
            await _context.SaveChangesAsync();

            return Ok(estadoServicioMod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var estadoServicio = await _context.EstadosServicio.FindAsync(id);
            if (estadoServicio == null) return NotFound();

            _context.EstadosServicio.Remove(estadoServicio);
            await _context.SaveChangesAsync();

            return NoContent();
                
        }
    }
}
