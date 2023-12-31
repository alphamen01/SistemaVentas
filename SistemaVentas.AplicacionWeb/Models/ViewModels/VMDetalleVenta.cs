﻿using Microsoft.EntityFrameworkCore;
using SistemaVentas.Entity.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.AplicacionWeb.Models.ViewModels
{
    public class VMDetalleVenta
    {
        //public int IdDetalleVenta { get; set; }

        //public int? IdVenta { get; set; }

        public int? IdProducto { get; set; }

        public string? MarcaProducto { get; set; }

        public string? DescripcionProducto { get; set; }

        public string? CategoriaProducto { get; set; }

        public int? Cantidad { get; set; }

        public string? Precio { get; set; }

        public string? Total { get; set; }

    }
}
