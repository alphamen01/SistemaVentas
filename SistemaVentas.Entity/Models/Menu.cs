using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Table("Menu")]
    public partial class Menu
    {
        public Menu()
        {
            InverseIdMenuPadreNavigation = new HashSet<Menu>();
            RolMenus = new HashSet<RolMenu>();
        }

        [Key]
        [Column("idMenu")]
        public int IdMenu { get; set; }

        [Column("descripcion")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Descripcion { get; set; }

        [Column("idMenuPadre")]
        public int? IdMenuPadre { get; set; }

        [Column("icono")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Icono { get; set; }

        [Column("controlador")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Controlador { get; set; }

        [Column("paginaAccion")]
        [StringLength(30)]
        [Unicode(false)]
        public string? PaginaAccion { get; set; }

        [Column("esActivo")]
        public bool? EsActivo { get; set; }

        [Column("fechaRegistro", TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }

        [ForeignKey("IdMenuPadre")]
        [InverseProperty("InverseIdMenuPadreNavigation")]
        public virtual Menu? IdMenuPadreNavigation { get; set; }

        [InverseProperty("IdMenuPadreNavigation")]
        public virtual ICollection<Menu> InverseIdMenuPadreNavigation { get; set; }

        [InverseProperty("IdMenuNavigation")]
        public virtual ICollection<RolMenu> RolMenus { get; set; }
    }
}
