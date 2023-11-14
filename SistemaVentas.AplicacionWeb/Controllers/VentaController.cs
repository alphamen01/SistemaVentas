using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.AplicacionWeb.Utilidades.Response;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.Entity.Models;
using SistemaVentas.BLL.Implements;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class VentaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITipoDocumentoVentaService _tipoDocumentoVentaService;
        private readonly IVentaService _ventaService;

        public VentaController(IMapper mapper, ITipoDocumentoVentaService tipoDocumentoVentaService, IVentaService ventaService)
        {
            _mapper = mapper;
            _ventaService = ventaService;
            _tipoDocumentoVentaService = tipoDocumentoVentaService;
        }

        public IActionResult NuevaVenta()
        {
            return View();
        }


        public IActionResult HistorialVenta()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaTipoDocumentoVenta()
        {
            var lista = await _tipoDocumentoVentaService.Lista();
            List<VMTipoDocumentoVenta> vMTipoDocumentoVentas = _mapper.Map<List<VMTipoDocumentoVenta>>(lista);
            return StatusCode(StatusCodes.Status200OK, vMTipoDocumentoVentas);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            var productos = await _ventaService.ObtenerProductos(busqueda);
            List<VMProducto> vMProductos = _mapper.Map<List<VMProducto>>(productos);
            return StatusCode(StatusCodes.Status200OK, vMProductos);
        }


        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VMVenta modelo)
        {
            GenericResponse<VMVenta> gResponse = new GenericResponse<VMVenta>();

            try
            {
                modelo.IdUsuario = 1;

                Venta venta_creada = await _ventaService.Registrar(_mapper.Map<Venta>(modelo));

                modelo = _mapper.Map<VMVenta>(venta_creada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;

            }
            catch(Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {

            var hventas = await _ventaService.Historial(numeroVenta,fechaInicio,fechaFin);
            List<VMVenta> vMHistorialVenta = _mapper.Map<List<VMVenta>>(hventas);
            return StatusCode(StatusCodes.Status200OK, vMHistorialVenta);
        }

    }
}
