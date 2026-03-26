namespace ServicioTecnicoAPI.DTOs.Equipo
{
    public class EquipoListDTO
    {
        public int Id { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public int TipoEquipoId { get; set; }
        public string TipoEquipoNombre { get; set; } = string.Empty;
        public bool TieneOrdenes { get; set; }
    }
}
