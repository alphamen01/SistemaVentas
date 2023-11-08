using Microsoft.EntityFrameworkCore;
using SistemaVentas.Entity.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.AplicacionWeb.Models.ViewModels
{
    public class VMCategoria
    {
        public int IdCategoria { get; set; }

        public string? Descripcion { get; set; }

        public int? EsActivo { get; set; }

    }
}
