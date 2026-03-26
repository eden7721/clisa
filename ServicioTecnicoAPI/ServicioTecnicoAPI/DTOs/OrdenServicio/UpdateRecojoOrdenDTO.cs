using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ServicioTecnicoAPI.DTOs.OrdenServicio
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateRecojoOrdenDTO : ControllerBase
    {
        [Required]
        public DateTime? FechaRecojo { get; set; } 
    }
}
