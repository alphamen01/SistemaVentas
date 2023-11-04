using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
            Venta = new HashSet<Venta>();
        }

        [Key]
        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("nombre")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [Column("correo")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Correo { get; set; }

        [Column("telefono")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Telefono { get; set; }

        [Column("idRol")]
        public int? IdRol { get; set; }

        [Column("urlFoto")]
        [StringLength(500)]
        [Unicode(false)]
        public string? UrlFoto { get; set; }

        [Column("nombreFoto")]
        [StringLength(100)]
        [Unicode(false)]
        public string? NombreFoto { get; set; }

        [Column("clave")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Clave { get; set; }

        [Column("esActivo")]
        public bool? EsActivo { get; set; }

        [Column("fechaRegistro", TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }

        [ForeignKey("IdRol")]
        [InverseProperty("Usuarios")]
        public virtual Rol? IdRolNavigation { get; set; }

        [InverseProperty("IdUsuarioNavigation")]
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
