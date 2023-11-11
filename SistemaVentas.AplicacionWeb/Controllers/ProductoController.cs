using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Newtonsoft.Json;
using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.AplicacionWeb.Utilidades.Response;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.Entity.Models;
using SistemaVentas.BLL.Implements;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class ProductoController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IProductoService _productoService;
        //private readonly ICategoriaService _categoriaService;

        public ProductoController(IMapper mapper, IProductoService productoService/*, ICategoriaService categoriaService*/)
        {
            _mapper = mapper;
            _productoService = productoService;
           // _categoriaService = categoriaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*[HttpGet]
        public async Task<IActionResult> ListaCategorias()
        {
            var lista = await _categoriaService.Lista();
            List<VMCategoria> vmListaCategorias = _mapper.Map<List<VMCategoria>>(lista);
            return StatusCode(StatusCodes.Status200OK, vmListaCategorias);
        }*/

        [HttpGet]
        public async Task<IActionResult> ListaProductos()
        {
            var lista = await _productoService.Lista();
            List<VMProducto> vmListaProductos = _mapper.Map<List<VMProducto>>(lista);
            return StatusCode(StatusCodes.Status200OK, new { data = vmListaProductos }); //por el formato del datatable de jquery
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile imagen, [FromForm] string modelo)
        {
            GenericResponse<VMProducto> gResponse = new GenericResponse<VMProducto>();

            try
            {
                VMProducto vmProducto = JsonConvert.DeserializeObject<VMProducto>(modelo)!;

                string nombreImagen = "";

                Stream imagenStream = null;

                if (imagen != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(imagen.FileName);
                    nombreImagen = string.Concat(nombre_en_codigo, extension);
                    imagenStream = imagen.OpenReadStream();
                }

                Producto producto_creado = await _productoService.Crear(_mapper.Map<Producto>(vmProducto), imagenStream!, nombreImagen);

                vmProducto = _mapper.Map<VMProducto>(producto_creado);

                gResponse.Estado = true;
                gResponse.Objeto = vmProducto;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile imagen, [FromForm] string modelo)
        {
            GenericResponse<VMProducto> gResponse = new GenericResponse<VMProducto>();

            try
            {
                VMProducto vmProducto = JsonConvert.DeserializeObject<VMProducto>(modelo)!;

                string nombreImagen = "";

                Stream imagenStream = null;

                if (imagen != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(imagen.FileName);
                    nombreImagen = string.Concat(nombre_en_codigo, extension);
                    imagenStream = imagen.OpenReadStream();
                }

                Producto producto_editado = await _productoService.Editar(_mapper.Map<Producto>(vmProducto), imagenStream!,nombreImagen);

                vmProducto = _mapper.Map<VMProducto>(producto_editado);

                gResponse.Estado = true;
                gResponse.Objeto = vmProducto;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idProducto)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _productoService.Eliminar(idProducto);

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
