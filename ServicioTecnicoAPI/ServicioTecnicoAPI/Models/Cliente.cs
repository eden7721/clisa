using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioTecnicoAPI.Models;


public class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;
    [Required]
    [MaxLength(8)]
    public string Dni { get; set; } = string.Empty;
    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}
