using SistemaVentas.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.Entity.Models;
using System.Globalization;
using SistemaVentas.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.BLL.Implements
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _genericDetalleVentaRepository;
        private readonly IGenericRepository<Categoria> _genericCategoriaRepository;
        private readonly IGenericRepository<Producto> _genericProductoRepository;
        private DateTime FechaInicio = DateTime.Now;

        public DashBoardService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> genericDetalleVentaRepository, IGenericRepository<Categoria> genericCategoriaRepository, IGenericRepository<Producto> genericProductoRepository)
        {
            _ventaRepository = ventaRepository;
            _genericDetalleVentaRepository = genericDetalleVentaRepository;
            _genericCategoriaRepository = genericCategoriaRepository;
            _genericProductoRepository = genericProductoRepository;

            FechaInicio = FechaInicio.AddDays(-7);
        }


        public async Task<int> TotalVentasUltimaSemana()
        {
            try
            {
                IQueryable<Venta> query = await _ventaRepository.Consultar(v => v.FechaRegistro!.Value.Date >= FechaInicio.Date);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> TotalIngresosUltimaSemana()
        {
            try
            {                

                IQueryable<Venta> query = await _ventaRepository.Consultar(v => v.FechaRegistro!.Value.Date >= FechaInicio.Date);
                decimal resultado = query.Select(v => v.Total)
                    .Sum(v => v.Value);
                return Convert.ToString(resultado, new CultureInfo("es-PE"));

            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TotalProductos()
        {
            try
            {
                IQueryable<Producto> query = await _genericProductoRepository.Consultar();
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TotalCategorias()
        {
            try
            {
                IQueryable<Categoria> query = await _genericCategoriaRepository.Consultar();
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }        

        public async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            try
            {
                IQueryable<Venta> query = await _ventaRepository.Consultar(v => v.FechaRegistro!.Value.Date >= FechaInicio.Date);
                
                Dictionary<string,int> resultado = query.GroupBy(v => v.FechaRegistro!.Value.Date).OrderByDescending(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() }).ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
                return resultado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> ProductosTopUltimaSemana()
        {
            try
            {
                IQueryable<DetalleVenta> query = await _genericDetalleVentaRepository.Consultar();

                Dictionary<string, int> resultado = query
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv => dv.IdVentaNavigation!.FechaRegistro!.Value.Date >= FechaInicio.Date)
                    .GroupBy(dv => dv.DescripcionProducto).OrderByDescending(g => g.Count())
                    .Select(dv => new { producto = dv.Key, total = dv.Count() }).ToDictionary(keySelector: r => r.producto, elementSelector: r => r.total);
                return resultado;
            }
            catch
            {
                throw;
            }
        }
    }
}
