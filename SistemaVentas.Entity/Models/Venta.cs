using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("Venta")]
    public partial class Venta
    {
        public Venta()
        {
            DetalleVenta = new HashSet<DetalleVenta>();
        }

        [Key]
        [Column("idVenta")]
        public int IdVenta { get; set; }
        [Column("numeroVenta")]
        [StringLength(6)]
        [Unicode(false)]
        public string? NumeroVenta { get; set; }
        [Column("idTipoDocumentoVenta")]
        public int? IdTipoDocumentoVenta { get; set; }
        [Column("idUsuario")]
        public int? IdUsuario { get; set; }
        [Column("documentoCliente")]
        [StringLength(10)]
        [Unicode(false)]
        public string? DocumentoCliente { get; set; }
        [Column("nombreCliente")]
        [StringLength(20)]
        [Unicode(false)]
        public string? NombreCliente { get; set; }
        [Column("subTotal", TypeName = "decimal(10, 2)")]
        public decimal? SubTotal { get; set; }
        [Column("impuestoTotal", TypeName = "decimal(10, 2)")]
        public decimal? ImpuestoTotal { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Total { get; set; }
        [Column("fechaRegistro", TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }

        [ForeignKey("IdTipoDocumentoVenta")]
        [InverseProperty("Venta")]
        public virtual TipoDocumentoVenta? IdTipoDocumentoVentaNavigation { get; set; }
        [ForeignKey("IdUsuario")]
        [InverseProperty("Venta")]
        public virtual Usuario? IdUsuarioNavigation { get; set; }
        [InverseProperty("IdVentaNavigation")]
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
