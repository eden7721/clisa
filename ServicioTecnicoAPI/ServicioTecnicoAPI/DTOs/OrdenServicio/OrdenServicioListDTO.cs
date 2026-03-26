using ServicioTecnicoAPI.Models;

namespace ServicioTecnicoAPI.DTOs.OrdenServicio
{
    public class OrdenServicioListDTO
    {
        public int Id { get; set; }
        public string NombreTipoServicio { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool Pagado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaRecojo { get; set; }
        public string NombreEstadoServicio { get; set; } = string.Empty;
        public int EstadoServicioId { get; set; }

    }
}
