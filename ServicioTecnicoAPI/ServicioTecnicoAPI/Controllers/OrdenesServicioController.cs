using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Data;
using ServicioTecnicoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioTecnicoAPI.DTOs.OrdenServicio;

namespace ServicioTecnicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesServicioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdenesServicioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrdenServicioListDTO>>> GetAll()
        {
            var ordenes = await _context.OrdenesServicio
                .Include(o => o.TipoServicio)
                .Include(o => o.EstadoServicio)
                .ToListAsync();

            var ordenesDTO = ordenes.Select(o => new OrdenServicioListDTO
            {
                Id = o.Id,
                Precio = o.Precio,
                Pagado = o.Pagado,
                FechaIngreso = o.FechaIngreso,
                FechaRecojo = o.FechaRecojo,
                NombreTipoServicio = o.TipoServicio!.Servicio,
                NombreEstadoServicio = o.EstadoServicio!.Estado,
                EstadoServicioId = o.EstadoServicioId
            }).ToList();

            return Ok(ordenesDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenServicioDTO>> GetById([FromRoute] int id)
        {
            var orden = await _context.OrdenesServicio
                .Include(o => o.Equipo)
                    .ThenInclude(e => e!.Marca)
                .Include(o => o.Equipo)
                    .ThenInclude(t => t!.TipoEquipo)
                .Include(o => o.TipoServicio)
                .Include(o => o.EstadoServicio)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null) return NotFound();

            var ordenDTO = new OrdenServicioDTO
            {
                Id = orden.Id,
                Precio = orden.Precio,
                Diagnostico = orden.Diagnostico,
                Observaciones = orden.Observaciones,
                Pagado = orden.Pagado,
                FechaIngreso = orden.FechaIngreso,
                FechaRecojo = orden.FechaRecojo,
                ModeloEquipo = orden.Equipo!.Modelo,
                NombreMarca = orden.Equipo.Marca!.Nombre,
                NombreTipoEquipo = orden.Equipo.TipoEquipo!.Nombre,
                NombreTipoServicio = orden.TipoServicio!.Servicio,
                NombreEstadoServicio = orden.EstadoServicio!.Estado
            };

            return Ok(ordenDTO);
        }

        [HttpPost]
        public async Task<ActionResult<OrdenServicioDTO>> Create([FromBody] CreateOrdenServicioDTO dto) 
        {
            var equipoExiste = await _context.Equipos.AnyAsync(e => e.Id == dto.EquipoId);
            var tipoServicioExiste = await _context.TiposServicio.AnyAsync(t => t.Id == dto.TipoServicioId);

            if (!equipoExiste || !tipoServicioExiste) return BadRequest("Equipo, TipoServicio no existen.");

            var orden = new OrdenServicio
            {
                Precio = dto.Precio,
                Diagnostico = dto.Diagnostico,
                Observaciones = dto.Observaciones,
                Pagado = dto.Pagado,
                FechaIngreso = DateTime.Now,
                EquipoId = dto.EquipoId,
                TipoServicioId = dto.TipoServicioId,
                EstadoServicioId = 1
            };

            _context.OrdenesServicio.Add(orden);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = orden.Id }, new { id = orden.Id});
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrdenServicioDTO>> Update([FromRoute] int id, [FromBody] CreateOrdenServicioDTO dto)
        {
            var orden = await _context.OrdenesServicio
                .Include(o => o.Equipo)
                    .ThenInclude(e => e!.Marca)
                .Include(o => o.Equipo)
                    .ThenInclude(e => e!.TipoEquipo)
                .Include(o => o.TipoServicio)
                .Include(o => o.EstadoServicio)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null) return NotFound();

            var equipoExiste = await _context.Equipos.AnyAsync(e => e.Id == dto.EquipoId);
            var tipoServicioExiste = await _context.TiposServicio.AnyAsync(t => t.Id == dto.TipoServicioId);

            if (!equipoExiste || !tipoServicioExiste) return BadRequest("Equipo, TipoServicio o Estado no existen");

            orden.Precio = dto.Precio;
            orden.Diagnostico = dto.Diagnostico;
            orden.Observaciones = dto.Observaciones;
            orden.Pagado = dto.Pagado;
            orden.EquipoId = dto.EquipoId;
            orden.TipoServicioId = dto.TipoServicioId;

            await _context.SaveChangesAsync();

            var ordenDTO = new OrdenServicioDTO
            {
                Id = orden.Id,
                Precio = orden.Precio,
                Diagnostico = orden.Diagnostico,
                Observaciones = orden.Observaciones,
                Pagado = orden.Pagado,
                FechaIngreso = orden.FechaIngreso,
                FechaRecojo = orden.FechaRecojo,
                ModeloEquipo = orden.Equipo!.Modelo,
                NombreMarca = orden.Equipo.Marca!.Nombre,
                NombreTipoEquipo = orden.Equipo.TipoEquipo!.Nombre,
                NombreEstadoServicio = orden.EstadoServicio!.Estado,
                NombreTipoServicio = orden.TipoServicio!.Servicio
            };
            return Ok(ordenDTO);
        }

        [HttpPatch("{id}/estado")]
        public async Task<ActionResult> UpdateEstado([FromRoute] int id, UpdateEstadoOrdenDTO dto)
        {
            var orden = await _context.OrdenesServicio.FindAsync(id);

            if (orden == null) return NotFound();

            var estadoExiste = await _context.EstadosServicio.AnyAsync(e => e.Id == dto.EstadoServicioId);

            if (!estadoExiste) return BadRequest("EstadoServicio no existe");

            orden.EstadoServicioId = dto.EstadoServicioId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/recojo")]
        public async Task<ActionResult> UpdateRecojo([FromRoute] int id)
        {
            var orden = await _context.OrdenesServicio.FindAsync(id);

            if (orden == null) return NotFound();

            orden.FechaRecojo = DateTime.Now;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/pago")]
        public async Task<ActionResult> UpdatePago([FromRoute] int id, UpdatePagoOrdenDTO dto)
        {
            var orden = await _context.OrdenesServicio.FindAsync(id);

            if (orden == null) return NotFound();

            orden.Pagado = dto.Pagado;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var orden = await _context.OrdenesServicio.FindAsync(id);

            if (orden == null) return NotFound();

            _context.OrdenesServicio.Remove(orden);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
