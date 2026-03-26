namespace ServicioTecnicoAPI.DTOs.Equipo
{
    public class EquipoDTO
    {
        public int Id { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set;  } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int MarcaId { get; set; }
        public string NombreMarca { get; set; } = string.Empty;
        public int TipoEquipoId { get; set; }
        public string NombreTipoEquipo { get; set; } = string.Empty;

    }
}
