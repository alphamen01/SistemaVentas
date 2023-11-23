using Microsoft.AspNetCore.Mvc;

using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.Entity.Models;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public AccesoController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public IActionResult RestablecerClave()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RestablecerClave(VMUsuarioLogin modelo)
        {
            try
            {

                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/RestablecerClave?clave=[clave]";

                bool resultado = await _usuarioService.RestablecerClave(modelo.Correo!, urlPlantillaCorreo);

                if (resultado)
                {
                    ViewData["Mensaje"] = "Listo, su contraseña fue restablecida. Revise su correo.";
                    ViewData["MensajeError"] = null;
                }
                else
                {
                    ViewData["Mensaje"] = null;
                    ViewData["MensajeError"] = "Tenemos problemas. Por favor intentelo de nuevo mas tarde.";
                }

            }catch (Exception ex)
            {
                ViewData["Mensaje"] = null;
                ViewData["MensajeError"] = ex.Message;
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin modelo)
        {
            Usuario usuario_encontrado = await _usuarioService.ObtnerPorCredenciales(modelo.Correo!, modelo.Clave!);

            if(usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias.";
                return View();
            }

            ViewData["Mensaje"] = null;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario_encontrado.IdRol.ToString()),
                new Claim("UrlFoto", usuario_encontrado.UrlFoto),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = modelo.MantenerSesion,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);


            return RedirectToAction("Index", "Home");   
        }
    }
}
