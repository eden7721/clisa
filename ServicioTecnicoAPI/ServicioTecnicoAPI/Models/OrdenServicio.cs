using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServicioTecnicoAPI.Models
{
    public class OrdenServicio
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal Precio { get; set; }

        [Required]
        [MaxLength(255)]
        public string Diagnostico { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Observaciones { get; set; } = string.Empty;

        public bool Pagado { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaRecojo { get; set; }

        public int TipoServicioId { get; set; }
        public TipoServicio? TipoServicio { get; set; }

        public int EquipoId { get; set; }
        public Equipo? Equipo { get; set; }

        public int EstadoServicioId { get; set; }
        public EstadoServicio? EstadoServicio { get; set; }
    }
}
