using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.BLL.Interfaces;
using AutoMapper;

namespace SistemaVentas.AplicacionWeb.Utilidades.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuViewComponent(IMenuService menuService, IMapper mapper)
        {
            _mapper = mapper;
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;

            List<VMMenu> listaMenus;

            if(claimsUser.Identity!.IsAuthenticated)
            {
                string idUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault()!;

                listaMenus = _mapper.Map<List<VMMenu>>(await _menuService.ObtenerMenus(int.Parse(idUsuario)));
            }
            else
            {
                listaMenus = new List<VMMenu> { };
            }

            return View(listaMenus);
            
        }
    }
}
