using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ServicioTecnicoAPI.DTOs.OrdenServicio
{
    public class UpdateEstadoOrdenDTO
    {
        [Required]
        public int EstadoServicioId { get; set; }
    }
}
