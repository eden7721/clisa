using System;
using System.Collections.Generic;
using System.Text;

namespace ServicioTecnicoAPI.Helpers
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errores { get; set; }

        public static ApiResponse<T> Success(T data, string mensaje = "Operación Exitosa")
        {
            return new ApiResponse<T>
            {
                Status = 200,
                Mensaje = mensaje,
                Data = data
            };
        }
        public static ApiResponse<T> Created(T data, string mensaje = "Recurso Creado")
        {
            return new ApiResponse<T>
            {
                Status = 201,
                Mensaje = mensaje,
                Data = data
            };
        }
        public static ApiResponse<T> Fail(int status, string mensaje, List<string>? errores = null)
        {
            return new ApiResponse<T>
            {
                Status = status,
                Mensaje = mensaje,
                Errores = errores
            };
        }
    }
}
