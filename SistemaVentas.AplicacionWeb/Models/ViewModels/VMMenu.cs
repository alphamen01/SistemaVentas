using Microsoft.EntityFrameworkCore;
using SistemaVentas.Entity.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.AplicacionWeb.Models.ViewModels
{
    public class VMMenu
    {
        //public int IdMenu { get; set; }

        public string? Descripcion { get; set; }

        public string? Icono { get; set; }

        public string? Controlador { get; set; }

        public string? PaginaAccion { get; set; }

        public virtual ICollection<VMMenu>? SubMenus { get; set; }

    }
}
