using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("NumeroCorrelativo")]
    public partial class NumeroCorrelativo
    {
        [Key]
        [Column("idNumeroCorrelativo")]
        public int IdNumeroCorrelativo { get; set; }
        [Column("ultimoNumero")]
        public int? UltimoNumero { get; set; }
        [Column("cantidadDigitos")]
        public int? CantidadDigitos { get; set; }
        [Column("gestion")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Gestion { get; set; }
        [Column("fechaActualizacion", TypeName = "datetime")]
        public DateTime? FechaActualizacion { get; set; }
    }
}
