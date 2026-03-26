using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServicioTecnicoAPI.DTOs.OrdenServicio
{
    public class UpdatePagoOrdenDTO
    {
        public bool Pagado { get; set; }
    }
}
