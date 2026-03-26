using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace ServicioTecnicoAPI.DTOs.OrdenServicio
{
    public class CreateOrdenServicioDTO
    {
        public decimal Precio { get; set; }
        [Required, MaxLength(255)]
        public string Diagnostico { get; set; } = string.Empty;
        [Required, MaxLength(255)]
        public string Observaciones { get; set; } = string.Empty;
        public bool Pagado { get; set; }
        
        public int EquipoId { get; set; }
        public int TipoServicioId { get; set; }
    }
}
