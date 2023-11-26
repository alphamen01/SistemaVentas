using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.AplicacionWeb.Utilidades.Response;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SistemaVentas.AplicacionWeb.Utilidades.CustomFilter;
//using DinkToPdf;
//using DinkToPdf.Contracts;
//using System.Net.Http;
//using static System.Net.WebRequestMethods;
//using Wkhtmltopdf.NetCore;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITipoDocumentoVentaService _tipoDocumentoVentaService;
        private readonly IVentaService _ventaService;
        //private readonly IConverter _converter;
        //private readonly HttpClient _httpClient;
        //private readonly IGeneratePdf _generatePdf;

        public VentaController(IMapper mapper, ITipoDocumentoVentaService tipoDocumentoVentaService, IVentaService ventaService/*, IConverter converter, HttpClient httpClient, IGeneratePdf generatePdf*/)
        {
            _mapper = mapper;
            _ventaService = ventaService;
            _tipoDocumentoVentaService = tipoDocumentoVentaService;
            //_converter = converter;
            //_httpClient = httpClient;
           // _generatePdf = generatePdf;
        }

        [ClaimRequirement(controlador: "Venta", accion: "NuevaVenta")]
        public IActionResult NuevaVenta()
        {
            return View();
        }


        [ClaimRequirement(controlador: "Venta", accion: "HistorialVenta")]
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
                ClaimsPrincipal claimsUser = HttpContext.User;

                string idUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault()!;

                modelo.IdUsuario = int.Parse(idUsuario);

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

        //public async Task<IActionResult> MostrarPDFVenta(string numeroVenta)
        //{
        //try
        //{
        //string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFVenta?numeroVenta={numeroVenta}";
        //////    var urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFVenta?numeroVenta={numeroVenta}"; // Reemplaza con tu URL real
        //////    var htmlContent = await GetHtmlContentFromUrl(urlPlantillaVista);

        //string urlPlantillaVista = $"http://localhost:5208/Home/VistaParaPDF";
        //string urlPlantillaVista = $"https://localhost:7029/Venta/HistorialVenta";
        //string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/Prueba";
        //string urlPlantillaVista = $"http://localhost:5208/Home/VistaParaPDF";
        

        //////    Console.WriteLine(urlPlantillaVista);
        //////    Console.WriteLine(htmlContent);


        //////var pdf = _generatePdf.GetPDF(htmlContent);
        //////var pdfStream = new System.IO.MemoryStream();
        //////pdfStream.Write(pdf, 0, pdf.Length);
        //////pdfStream.Position = 0;
        //////return new FileStreamResult(pdfStream, "application/pdf");

        //var pdf = new HtmlToPdfDocument()
        //{

        //    GlobalSettings = new GlobalSettings()
        //    {
        //        ColorMode = ColorMode.Color,
        //        PaperSize = PaperKind.A4,
        //        Orientation = Orientation.Portrait,


        //    },


        //    Objects =
        //    {
        //        new ObjectSettings()
        //        {
        //           PagesCount = true,  
        //           //Page = urlPlantillaVista,
        //           HtmlContent = htmlContent,
        //           WebSettings = { 
        //                DefaultEncoding = "utf-8",
        //                LoadImages = true,
        //                EnableIntelligentShrinking = true, 
        //            },
        //           UseExternalLinks = true,
        //           LoadSettings = new LoadSettings()
        //           {
        //               BlockLocalFileAccess = false,

        //           },

        //        }
        //    }
        //};

        //Console.WriteLine(pdf);

        //var archivoPDF = _converter.Convert(pdf);







        //return File(archivoPDF, "application/pdf");
        //}


        //}


        //}

        public IActionResult MostrarPDFVenta(string numeroVenta)
        {
            var urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFVenta?numeroVenta={numeroVenta}";
            return Redirect(urlPlantillaVista);
        }

        //private async Task<string> GetHtmlContentFromUrl(string url)
        //{
        //    using (var response = await _httpClient.GetAsync(url))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        return await response.Content.ReadAsStringAsync();
        //    }
        //}

    }
}
