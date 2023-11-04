using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("TipoDocumentoVenta")]
    public partial class TipoDocumentoVenta
    {
        public TipoDocumentoVenta()
        {
            Venta = new HashSet<Venta>();
        }

        [Key]
        [Column("idTipoDocumentoVenta")]
        public int IdTipoDocumentoVenta { get; set; }

        [Column("descripcion")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        [Column("esActivo")]
        public bool? EsActivo { get; set; }

        [Column("fechaRegistro", TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }

        [InverseProperty("IdTipoDocumentoVentaNavigation")]
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
