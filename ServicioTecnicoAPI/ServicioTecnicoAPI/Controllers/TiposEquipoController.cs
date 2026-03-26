using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Models;
using ServicioTecnicoAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServicioTecnicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposEquipoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposEquipoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoEquipo>>> GetAll()
        {
            var tipos = await _context.TiposEquipo.ToListAsync();

            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoEquipo>> GetById([FromRoute] int id)
        {
            var tipo = await _context.TiposEquipo.FindAsync(id);

            if (tipo == null) return NotFound();

            return Ok(tipo);
        }

        [HttpPost]
        public async Task<ActionResult<TipoEquipo>> Post([FromBody] TipoEquipo tipoEquipo)
        {
            _context.TiposEquipo.Add(tipoEquipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tipoEquipo.Id }, tipoEquipo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TipoEquipo>> Update([FromRoute] int id, [FromBody] TipoEquipo tipoEquipo)
        {
            var tipoUpdated = await _context.TiposEquipo.FindAsync(id);

            if(tipoUpdated == null) return NotFound();

            tipoUpdated.Nombre = tipoEquipo.Nombre;

            await _context.SaveChangesAsync();

            return Ok(tipoUpdated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            var tipoBorrado = await _context.TiposEquipo.FindAsync(id);
            if (tipoBorrado == null) return NotFound();

            _context.TiposEquipo.Remove(tipoBorrado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
