
namespace ServicioTecnicoAPI.DTOs.OrdenServicio
{
    public class OrdenServicioDTO
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public bool Pagado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaRecojo { get; set; }
        public string ModeloEquipo { get; set; } = string.Empty;
        public string NombreMarca { get; set; } = string.Empty;
        public string NombreTipoEquipo { get; set; } = string.Empty;
        public string NombreTipoServicio { get; set; } = string.Empty;
        public string NombreEstadoServicio { get; set; } = string.Empty;
    }
}
