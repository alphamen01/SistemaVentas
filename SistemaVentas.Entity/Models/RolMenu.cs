using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("RolMenu")]
    public partial class RolMenu
    {
        [Key]
        [Column("idRolMenu")]
        public int IdRolMenu { get; set; }
        [Column("idRol")]
        public int? IdRol { get; set; }
        [Column("idMenu")]
        public int? IdMenu { get; set; }
        [Column("esActivo")]
        public bool? EsActivo { get; set; }
        [Column("fechaRegistro", TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }

        [ForeignKey("IdMenu")]
        [InverseProperty("RolMenus")]
        public virtual Menu? IdMenuNavigation { get; set; }
        [ForeignKey("IdRol")]
        [InverseProperty("RolMenus")]
        public virtual Rol? IdRolNavigation { get; set; }
    }
}
