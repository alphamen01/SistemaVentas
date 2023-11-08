using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.AplicacionWeb.Models.ViewModels
{
    public class VMTipoDocumentoVenta
    {
        public int IdTipoDocumentoVenta { get; set; }

        public string? Descripcion { get; set; }

        //public int? EsActivo { get; set; }
    }
}
