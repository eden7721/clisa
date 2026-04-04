using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Data;
using ServicioTecnicoAPI.DTOs.Cliente;
using ServicioTecnicoAPI.Helpers;
using ServicioTecnicoAPI.Models;

namespace ServicioTecnicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //Hace ímplicitos los atributos [FromBody], [FromRoute], [FromQuery]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }
        //GET
        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> GetAll()
        {
            var clientes = await _context.Clientes
                .Include(e => e.Equipos)
                .ToListAsync();

            var clientesDTO = clientes.Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                DNI = c.Dni,
                Telefono = c.Telefono,
                TieneEquipos = c.Equipos.Any()
            }).ToList();

            return Ok(ApiResponse<List<ClienteDTO>>.Success(clientesDTO));
        }

        //GET{id}
        [HttpGet("{id}")] //atributo ímplicito = [FromRoute]
        public async Task<ActionResult<ClienteDTO>> GetById(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Equipos)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null) return NotFound(ApiResponse<ClienteDTO>.Fail(404, "Cliente no econtrado"));

            var clienteDTO = new ClienteDTO()
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                DNI = cliente.Dni,
                Telefono = cliente.Telefono,
                TieneEquipos = cliente.Equipos.Any()
            };

            return Ok(ApiResponse<ClienteDTO>.Success(clienteDTO));
        }

        //POST
        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> Create(CreateClienteDTO dto) //Atributo ímplicito = [FromBody]
        {
            var cliente = new Cliente()
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Dni = dto.DNI,
                Telefono = dto.Telefono
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var clienteDTO = new ClienteDTO()
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                DNI = cliente.Dni,
                Telefono = cliente.Telefono,
                TieneEquipos = false
            };

            return CreatedAtAction(nameof(GetById), new {id = cliente.Id},
                 ApiResponse<ClienteDTO>.Created(clienteDTO));
        }

        //PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteDTO>> Update(int id, CreateClienteDTO dto)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Equipos)
                .FirstOrDefaultAsync();

            if (cliente == null) return NotFound(ApiResponse<ClienteDTO>.Fail(404, "Cliente no encontrado"));

            cliente.Nombre = dto.Nombre;
            cliente.Apellido = dto.Apellido;
            cliente.Dni = dto.DNI;
            cliente.Telefono = dto.Telefono;

            await _context.SaveChangesAsync();

            var clienteDTO = new ClienteDTO()
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                DNI = cliente.Dni,
                Telefono = cliente.Telefono,
                TieneEquipos = cliente.Equipos.Any()
            };

            return Ok(ApiResponse<ClienteDTO>.Success(clienteDTO, "Cliente actualizado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if(cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<Object>.Success(null!, "Cliente eliminado exitosamente"));
        }


    }
}
