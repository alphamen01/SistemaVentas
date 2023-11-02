using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("Negocio")]
    public partial class Negocio
    {
        [Key]
        [Column("idNegocio")]
        public int IdNegocio { get; set; }
        [Column("urlLogo")]
        [StringLength(500)]
        [Unicode(false)]
        public string? UrlLogo { get; set; }
        [Column("nombreLogo")]
        [StringLength(100)]
        [Unicode(false)]
        public string? NombreLogo { get; set; }
        [Column("numeroDocumento")]
        [StringLength(50)]
        [Unicode(false)]
        public string? NumeroDocumento { get; set; }
        [Column("nombre")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Nombre { get; set; }
        [Column("correo")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Correo { get; set; }
        [Column("direccion")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Direccion { get; set; }
        [Column("telefono")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Telefono { get; set; }
        [Column("porcentajeImpuesto", TypeName = "decimal(10, 2)")]
        public decimal? PorcentajeImpuesto { get; set; }
        [Column("simboloMoneda")]
        [StringLength(5)]
        [Unicode(false)]
        public string? SimboloMoneda { get; set; }
    }
}
