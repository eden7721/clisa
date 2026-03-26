using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Data;
using ServicioTecnicoAPI.Models;
using ServicioTecnicoAPI.DTOs.Equipo;

namespace ServicioTecnicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {

        private readonly AppDbContext _context;

        public EquiposController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EquipoListDTO>>> GetAll()
        {
            var equipos = await _context.Equipos
                .Include(e => e.Cliente)
                .Include(e => e.Marca)
                .Include(e => e.TipoEquipo)
                .Include(e => e.OrdenesServicio)
                .ToListAsync();

            var equiposDTO = equipos.Select(e => new EquipoListDTO
            {
                Id = e.Id,
                Modelo = e.Modelo,
                NumeroSerie = e.NumeroSerie,
                TipoEquipoId = e.TipoEquipoId,
                TieneOrdenes = e.OrdenesServicio.Any(),
                TipoEquipoNombre = e.TipoEquipo!.Nombre
            });
            return Ok(equiposDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipoDTO>> GetById([FromRoute] int id)
        {
            var equipo = await _context.Equipos
                .Include(e => e.Cliente)
                .Include(e => e.Marca)
                .Include(e => e.TipoEquipo)
                .Include(e => e.OrdenesServicio)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (equipo == null) return NotFound();

            var equipoDTO = new EquipoDTO()
            {
                Id = equipo.Id,
                Modelo = equipo.Modelo,
                NumeroSerie = equipo.NumeroSerie,
                Detalles = equipo.Detalles,
                ClienteId = equipo.ClienteId,
                NombreCliente = $"{equipo.Cliente!.Nombre} {equipo.Cliente!.Nombre}",
                MarcaId = equipo.MarcaId,
                NombreMarca = equipo.Marca!.Nombre,
                TipoEquipoId = equipo.TipoEquipoId,
                NombreTipoEquipo = equipo.TipoEquipo!.Nombre,
            };

            return Ok(equipoDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EquipoDTO>> Post([FromBody] CreateEquipoDTO dto)
        {
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            var marcaExiste = await _context.Marcas.AnyAsync(c => c.Id == dto.MarcaId);
            var tipoEquipoExiste = await _context.TiposEquipo.AnyAsync(c => c.Id == dto.TipoEquipoId);

            if (!clienteExiste || !marcaExiste || !tipoEquipoExiste) return BadRequest("Cliente, Marca o TipoEquipo no existe");

            var equipo = new Equipo()
            {
                Modelo = dto.Modelo,
                NumeroSerie = dto.NumeroSerie,
                Detalles = dto.Detalles,
                ClienteId = dto.ClienteId,
                MarcaId = dto.MarcaId,
                TipoEquipoId = dto.TipoEquipoId
            };

            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = equipo.Id}, new {id = equipo.Id});
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EquipoDTO>> Update([FromRoute] int id, [FromBody] CreateEquipoDTO dto)
        {
            var equipo = await _context.Equipos
                .Include(e => e.Cliente)
                .Include(e => e.Marca)
                .Include(e => e.TipoEquipo)
                .Include(e => e.OrdenesServicio)
                .FirstOrDefaultAsync();

            if (equipo == null) return NotFound();

            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            var marcaExiste = await _context.Marcas.AnyAsync(m => m.Id == dto.MarcaId);
            var tipoExiste = await _context.TiposEquipo.AnyAsync(t => t.Id == dto.TipoEquipoId);

            if (!clienteExiste || !marcaExiste || !tipoExiste) return BadRequest("Cliente, Marca o TipoEquipo no existe.");

            equipo.Modelo = dto.Modelo;
            equipo.NumeroSerie = dto.NumeroSerie;
            equipo.Detalles = dto.Detalles;
            equipo.ClienteId = dto.ClienteId;
            equipo.MarcaId = dto.MarcaId;
            equipo.TipoEquipoId = dto.TipoEquipoId;

            await _context.SaveChangesAsync();

            var equipoDTO = new EquipoDTO()
            {
                Id = equipo.Id,
                Modelo = equipo.Modelo,
                NumeroSerie = equipo.NumeroSerie,
                Detalles = equipo.Detalles,
                ClienteId = equipo.ClienteId,
                NombreCliente = $"{equipo.Cliente!.Nombre} {equipo.Cliente.Apellido}",
                MarcaId = equipo.MarcaId,
                NombreMarca = equipo.Marca!.Nombre,
                TipoEquipoId = equipo.TipoEquipoId,
                NombreTipoEquipo = equipo.TipoEquipo!.Nombre,
            };

            return Ok(equipoDTO);
                
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);

            if(equipo == null) return NotFound();

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
 