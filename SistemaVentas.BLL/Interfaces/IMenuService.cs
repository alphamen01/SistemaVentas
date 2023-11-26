using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVentas.Entity.Models;

namespace SistemaVentas.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> ObtenerMenus(int idUsuario);
        Task<bool> TienePermisoMenu(int idUsuario, string controlador, string accion);
    }
}
