using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Microsoft.Build.Framework;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class PlantillaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INegocioService _negocioService;
        private readonly IVentaService _ventaService;


        public PlantillaController(IMapper mapper, INegocioService negocioService, IVentaService ventaService)
        {
            _mapper = mapper;
            _negocioService = negocioService;
            _ventaService = ventaService;
        }

        public IActionResult EnviarClave(string correo, string clave)
        {
            ViewData["Correo"] = correo;
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";
            return View();
        }

        
        public async Task<IActionResult> PDFVenta(string numeroVenta)
        {
            VMVenta vMVenta = _mapper.Map<VMVenta>(await _ventaService.Detalle(numeroVenta));
            VMNegocio vMNegocio = _mapper.Map<VMNegocio>(await _negocioService.Obtener());

            VMPDFVenta modelo = new VMPDFVenta();
            modelo.negocio = vMNegocio;
            modelo.venta = vMVenta;

            //return View(modelo);

            var pdf = new ViewAsPdf("PDFVenta", modelo)
            {
                //FileName = $"NumeroVenta-{numeroVenta}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--enable-local-file-access",
            };


            return pdf;

        }

        public IActionResult RestablecerClave(string clave)
        {
            ViewData["Clave"] = clave;
            return View();
        }
    }
}
