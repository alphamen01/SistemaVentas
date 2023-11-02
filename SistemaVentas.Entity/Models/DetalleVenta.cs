using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("DetalleVenta")]
    public partial class DetalleVenta
    {
        [Key]
        [Column("idDetalleVenta")]
        public int IdDetalleVenta { get; set; }
        [Column("idVenta")]
        public int? IdVenta { get; set; }
        [Column("idProducto")]
        public int? IdProducto { get; set; }
        [Column("marcaProducto")]
        [StringLength(100)]
        [Unicode(false)]
        public string? MarcaProducto { get; set; }
        [Column("descripcionProducto")]
        [StringLength(100)]
        [Unicode(false)]
        public string? DescripcionProducto { get; set; }
        [Column("categoriaProducto")]
        [StringLength(100)]
        [Unicode(false)]
        public string? CategoriaProducto { get; set; }
        [Column("cantidad")]
        public int? Cantidad { get; set; }
        [Column("precio", TypeName = "decimal(10, 2)")]
        public decimal? Precio { get; set; }
        [Column("total", TypeName = "decimal(10, 2)")]
        public decimal? Total { get; set; }

        [ForeignKey("IdVenta")]
        [InverseProperty("DetalleVenta")]
        public virtual Venta? IdVentaNavigation { get; set; }
    }
}
