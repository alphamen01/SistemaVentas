﻿using SistemaVentas.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SistemaVentas.DAL.Context;
using SistemaVentas.Entity;
using SistemaVentas.Entity.Models;


namespace SistemaVentas.DAL.Implements
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {

        private readonly DBVENTAContext _dbContext;

        public VentaRepository(DBVENTAContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta entidad)
        {
           Venta ventagenerada = new Venta();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in entidad.DetalleVenta)
                    {
                        Producto producto_encontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _dbContext.Productos.Update(producto_encontrado);
                    }

                    await _dbContext.SaveChangesAsync();

                    NumeroCorrelativo correlativo = _dbContext.NumeroCorrelativos.Where(n => n.Gestion == "venta").First();

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaActualizacion = DateTime.Now;

                    _dbContext.NumeroCorrelativos.Update(correlativo);
                    await _dbContext.SaveChangesAsync();

                    string ceros = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos.Value));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);

                    entidad.NumeroVenta = numeroVenta;

                    await _dbContext.Ventas.AddAsync(entidad);
                    await _dbContext.SaveChangesAsync();

                    ventagenerada = entidad;

                    transaction.Commit();


                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

                return ventagenerada;
            }
        }

        public async Task<List<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin)
        {
            List<DetalleVenta> listResumen = await _dbContext.DetalleVenta
                                                              .Include(v => v.IdVentaNavigation)
                                                              .ThenInclude(u => u.IdUsuarioNavigation)
                                                              .Include(v => v.IdVentaNavigation)
                                                              .ThenInclude(tdv => tdv.IdTipoDocumentoVentaNavigation)
                                                              .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= FechaInicio.Date &&
                                                              dv.IdVentaNavigation.FechaRegistro.Value.Date <= FechaFin.Date).ToListAsync();

            return listResumen;


        }
    }
}
