using Microsoft.AspNetCore.Mvc;


using AutoMapper;
using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SistemaVentas.AplicacionWeb.Utilidades.CustomFilter;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    [Authorize]
    public class ReporteController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IVentaService _ventaService;

        public ReporteController(IMapper mapper, IVentaService ventaService)
        {
            _mapper = mapper;
            _ventaService = ventaService;
        }

        [ClaimRequirement(controlador: "Reporte", accion: "Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReporteVenta(string fechaInicio, string fechaFin)
        {
            var reporte = await _ventaService.Reporte(fechaInicio, fechaFin);
            List<VMReporteVenta> vmLista = _mapper.Map<List<VMReporteVenta>>(reporte);

            return StatusCode(StatusCodes.Status200OK, new { data = vmLista });

        }
    }
}
