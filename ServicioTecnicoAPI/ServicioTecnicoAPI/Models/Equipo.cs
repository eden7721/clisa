using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioTecnicoAPI.Models
{
    public class Equipo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NumeroSerie { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Detalles { get; set; } = string.Empty;

        [Required]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        [Required]
        public int MarcaId { get; set; }
        public Marca? Marca { get; set; }

        [Required]
        public int TipoEquipoId { get; set; }
        public TipoEquipo? TipoEquipo { get; set; }

        public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
        
    }
}
