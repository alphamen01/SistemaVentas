using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("Rol")]
    public partial class Rol
    {
        public Rol()
        {
            RolMenus = new HashSet<RolMenu>();
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        [Column("idRol")]
        public int IdRol { get; set; }
        [Column("descripcion")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Descripcion { get; set; }
        [Column("esActivo")]
        public bool? EsActivo { get; set; }
        [Column("fechaRegistro", TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }

        [InverseProperty("IdRolNavigation")]
        public virtual ICollection<RolMenu> RolMenus { get; set; }
        [InverseProperty("IdRolNavigation")]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
