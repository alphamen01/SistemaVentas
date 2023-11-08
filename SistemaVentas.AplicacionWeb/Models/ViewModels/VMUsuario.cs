using Microsoft.EntityFrameworkCore;
using SistemaVentas.Entity.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.AplicacionWeb.Models.ViewModels
{
    public class VMUsuario
    {
        public int IdUsuario { get; set; }

        public string? Nombre { get; set; }

        public string? Correo { get; set; }

        public string? Telefono { get; set; }

        public int? IdRol { get; set; }

        public string? NombreRol { get; set; }

        public string? UrlFoto { get; set; }

        public int? EsActivo { get; set; }
                
    }
}
