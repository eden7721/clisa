using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Data;
using ServicioTecnicoAPI.Models;
using ServicioTecnicoAPI.DTOs.Equipo;
using ServicioTecnicoAPI.Helpers;

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
        public async Task<ActionResult<ApiResponse<List<EquipoListDTO>>>> GetAll(
            [FromQuery] int? clienteId,
            [FromQuery] int? marcaId,
            [FromQuery] int? tipoEquipoId)
        {
            var query = _context.Equipos
                    .Include(e => e.Cliente)
                    .Include(e => e.Marca)
                    .Include(e => e.TipoEquipo)
                    .Include(e => e.OrdenesServicio)
                    .AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(q => q.ClienteId == clienteId);

            if(marcaId.HasValue)
                query = query.Where(q => q.MarcaId == marcaId);

            if(tipoEquipoId.HasValue)
                query = query.Where(q => q.TipoEquipoId == tipoEquipoId);

            var equipos = await query.ToListAsync();

            var equiposDTO = equipos.Select(e => new EquipoListDTO
            {
                Id = e.Id,
                Modelo = e.Modelo,
                NumeroSerie = e.NumeroSerie,
                TipoEquipoId = e.TipoEquipoId,
                TieneOrdenes = e.OrdenesServicio.Any(),
                TipoEquipoNombre = e.TipoEquipo!.Nombre
            }).ToList();
            return Ok(ApiResponse<List<EquipoListDTO>>.Success(equiposDTO));
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

            return Ok(ApiResponse<EquipoDTO>.Success(equipoDTO));
        }

        [HttpPost]
        public async Task<ActionResult<EquipoDTO>> Post([FromBody] CreateEquipoDTO dto)
        {
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            var marcaExiste = await _context.Marcas.AnyAsync(c => c.Id == dto.MarcaId);
            var tipoEquipoExiste = await _context.TiposEquipo.AnyAsync(c => c.Id == dto.TipoEquipoId);

            if (!clienteExiste || !marcaExiste || !tipoEquipoExiste) return BadRequest(ApiResponse<EquipoDTO>.Fail(400, "Cliente, Marca o TipoEquipo no existe"));

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

            return CreatedAtAction(nameof(GetById), new { id = equipo.Id}, ApiResponse<CreateEquipoDTO>.Created(dto));
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

            if (equipo == null) return NotFound(ApiResponse<EquipoDTO>.Fail(404, "Equipo no encontrado"));

            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            var marcaExiste = await _context.Marcas.AnyAsync(m => m.Id == dto.MarcaId);
            var tipoExiste = await _context.TiposEquipo.AnyAsync(t => t.Id == dto.TipoEquipoId);

            if (!clienteExiste || !marcaExiste || !tipoExiste) return BadRequest(ApiResponse<EquipoDTO>.Fail(400, "Cliente, Marca o TipoEquipo no existe."));

            equipo.Modelo = dto.Modelo;
            equipo.NumeroSerie = dto.NumeroSerie;
            equipo.Detalles = dto.Detalles;
            equipo.ClienteId = dto.ClienteId;
            equipo.MarcaId = dto.MarcaId;
            equipo.TipoEquipoId = dto.TipoEquipoId;

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<CreateEquipoDTO>.Success(dto, "cliente modificado exitosamente"));
                
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);

            if(equipo == null) return NotFound();

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<Object>.Success(null!, "Equipo eliminado"));

        }
    }
}
 