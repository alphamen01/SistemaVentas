using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.Entity.Models
{
    [Keyless]
    [Table("Configuracion")]
    public partial class Configuracion
    {
        [Column("recurso")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Recurso { get; set; }

        [Column("propiedad")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Propiedad { get; set; }

        [Column("valor")]
        [StringLength(60)]
        [Unicode(false)]
        public string? Valor { get; set; }
    }
}
