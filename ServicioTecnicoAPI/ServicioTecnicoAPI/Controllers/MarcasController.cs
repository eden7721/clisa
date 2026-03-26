using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Models;
using ServicioTecnicoAPI.Data;

namespace ServicioTecnicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MarcasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Marca>>> GetAll()
        {
            var marcas = await _context.Marcas.ToListAsync();
            return Ok(marcas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Marca>> GetForId(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null) return NotFound();
            return Ok(marca);
        }

        [HttpPost]
        public async Task<ActionResult<Marca>> Post([FromBody] Marca marca)
        {
            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetForId), new { id = marca.Id }, marca);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Marca>> Update([FromRoute] int id, [FromBody] Marca marca)
        {
            var marcaExistente = await _context.Marcas.FindAsync(id);
            if(marcaExistente == null) return NotFound();
            marcaExistente.Nombre = marca.Nombre;
            await _context.SaveChangesAsync();
            return Ok(marcaExistente);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null) return NotFound();

            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
