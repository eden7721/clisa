using System.ComponentModel.DataAnnotations;

namespace ServicioTecnicoAPI.DTOs.Equipo
{
    public class CreateEquipoDTO
    {
        [Required]
        [MaxLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NumeroSerie { get; set;  } = string.Empty;

        [MaxLength(255)]
        public string Detalles { get; set; } = string.Empty;

        public int MarcaId { get; set; }

        public int ClienteId { get; set; }

        public int TipoEquipoId { get; set; }


    }
}
