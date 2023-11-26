using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaVentas.BLL.Interfaces;
using System.Security.Claims;

namespace SistemaVentas.AplicacionWeb.Utilidades.CustomFilter
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private string _controlador;
        private string _accion;
        private IMenuService _menuService;

        public ClaimRequirementFilter(string controlador, string accion, IMenuService menuService)
        {
            _accion = accion;
            _controlador = controlador;
            _menuService = menuService;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            ClaimsPrincipal claimsUser = context.HttpContext.User;

            string idUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault()!;

            bool tiene_permiso = await _menuService.TienePermisoMenu(int.Parse(idUsuario), _controlador, _accion);


            if (!tiene_permiso)
            {
                context.Result = new RedirectToActionResult("SinPermiso", "Home", null);
            }
        }
    }
}
