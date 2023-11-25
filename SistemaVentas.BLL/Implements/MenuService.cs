﻿using SistemaVentas.BLL.Interfaces;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Implements
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Menu> _repositoryMenu;
        private readonly IGenericRepository<RolMenu> _repositoryRolMenu;
        private readonly IGenericRepository<Usuario> _repositoryUsuario;

        public MenuService(IGenericRepository<Menu> repositoryMenu, IGenericRepository<RolMenu> repositoryRolMenu, IGenericRepository<Usuario> repositoryUsuario)
        {
            _repositoryMenu = repositoryMenu;
            _repositoryRolMenu = repositoryRolMenu;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<List<Menu>> ObtenerMenus(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _repositoryUsuario.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<RolMenu> tbRolMenu = await _repositoryRolMenu.Consultar();
            IQueryable<Menu> tbMenu = await _repositoryMenu.Consultar();


            IQueryable<Menu> MenuPadre = (from u  in tbUsuario 
                                          join rm in tbRolMenu on u.IdRol equals rm.IdRol
                                          join m in tbMenu on rm.IdMenu equals m.IdMenu
                                          join mpadre in tbMenu on m.IdMenuPadre equals mpadre.IdMenu
                                          select mpadre).Distinct().AsQueryable();

            IQueryable<Menu> MenuHijos = (from u in tbUsuario
                                         join rm in tbRolMenu on u.IdRol equals rm.IdRol
                                         join m in tbMenu on rm.IdMenu equals m.IdMenu
                                         where m.IdMenu != m.IdMenuPadre
                                         select m).Distinct().AsQueryable();

            List<Menu> listaMenu = (from mpadre  in MenuPadre
                                    select new Menu()
                                    {
                                        Descripcion = mpadre.Descripcion,
                                        Icono = mpadre.Icono,
                                        Controlador = mpadre.Controlador,
                                        PaginaAccion = mpadre.PaginaAccion,
                                        InverseIdMenuPadreNavigation = (from mhijo in MenuHijos
                                                                        where mhijo.IdMenuPadre  == mpadre.IdMenu
                                                                        select mhijo).ToList()
                                    }).ToList();

            return listaMenu;
        }
    }
}
