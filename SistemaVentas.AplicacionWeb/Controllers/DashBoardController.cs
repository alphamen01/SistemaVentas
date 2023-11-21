using Microsoft.AspNetCore.Mvc;

using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.AplicacionWeb.Utilidades.Response;
using SistemaVentas.BLL.Interfaces;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IDashBoardService _dashboardService;

        public DashBoardController(IDashBoardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {
            GenericResponse<VMDashBoard> gResponse = new GenericResponse<VMDashBoard>();

            try
            {
                VMDashBoard vMDashBoard = new VMDashBoard();

                vMDashBoard.TotalVentas = await _dashboardService.TotalVentasUltimaSemana();

                vMDashBoard.TotalIngresos = await _dashboardService.TotalIngresosUltimaSemana();

                vMDashBoard.TotalProductos = await _dashboardService.TotalProductos();

                vMDashBoard.TotalCategorias = await _dashboardService.TotalCategorias();

                List<VMVentasSemana> listaVentasSemana = new List<VMVentasSemana>();
                List<VMProductosSemana> listaProductosSemana = new List<VMProductosSemana>();

                foreach(KeyValuePair<string, int> item in await _dashboardService.VentasUltimaSemana())
                {
                    listaVentasSemana.Add(new VMVentasSemana()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                foreach (KeyValuePair<string, int> item in await _dashboardService.ProductosTopUltimaSemana())
                {
                    listaProductosSemana.Add(new VMProductosSemana()
                    {
                        Producto = item.Key,
                        Cantidad = item.Value
                    });
                }

                vMDashBoard.VentasUltimaSemana = listaVentasSemana;

                vMDashBoard.ProductosTopUltimaSemana = listaProductosSemana;

                gResponse.Estado = true;
                gResponse.Objeto = vMDashBoard;
            }
            catch(Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
