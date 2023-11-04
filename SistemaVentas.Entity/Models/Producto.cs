using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("Producto")]
    public partial class Producto
    {
        [Key]
        [Column("idProducto")]
        public int IdProducto { get; set; }

        [Column("codigoBarra")]
        [StringLength(50)]
        [Unicode(false)]
        public string? CodigoBarra { get; set; }

        [Column("marca")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Marca { get; set; }

        [Column("descripcion")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        [Column("idCategoria")]
        public int? IdCategoria { get; set; }

        [Column("stock")]
        public int? Stock { get; set; }

        [Column("urlImagen")]
        [StringLength(500)]
        [Unicode(false)]
        public string? UrlImagen { get; set; }

        [Column("nombreImagen")]
        [StringLength(100)]
        [Unicode(false)]
        public string? NombreImagen { get; set; }

        [Column("precio", TypeName = "decimal(10, 2)")]
        public decimal? Precio { get; set; }

        [Column("esActivo")]
        public bool? EsActivo { get; set; }

        [Column("fechaRegistro", TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }

        [ForeignKey("IdCategoria")]
        [InverseProperty("Productos")]
        public virtual Categoria? IdCategoriaNavigation { get; set; }
    }
}
