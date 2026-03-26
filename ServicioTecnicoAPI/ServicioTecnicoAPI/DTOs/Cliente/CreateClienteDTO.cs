using System.ComponentModel.DataAnnotations;
namespace ServicioTecnicoAPI.DTOs.Cliente
{
    public class CreateClienteDTO
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [MaxLength(8)]
        public string DNI { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;



    }
}
