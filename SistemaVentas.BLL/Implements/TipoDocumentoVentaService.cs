using SistemaVentas.BLL.Interfaces;
using SistemaVentas.DAL.Implements;
using SistemaVentas.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Implements
{
    public class TipoDocumentoVentaService : ITipoDocumentoVentaService
    {
        private readonly GenericRepository<TipoDocumentoVenta> _repositorio;

        public TipoDocumentoVentaService(GenericRepository<TipoDocumentoVenta> repositorio)
        {
            _repositorio = repositorio;
        }


        public async Task<List<TipoDocumentoVenta>> Lista()
        {
            IQueryable<TipoDocumentoVenta> query = await _repositorio.Consultar();
            return query.ToList();
        }
    }
}
