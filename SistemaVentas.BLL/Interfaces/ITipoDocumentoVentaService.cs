using SistemaVentas.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Interfaces
{
    public interface ITipoDocumentoVentaService
    {
        Task<List<TipoDocumentoVenta>> Lista();
    }
}
