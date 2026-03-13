using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;
using MiTienda.Utilities;

namespace MiTienda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CategoriaService _categoriaService;
        private readonly MarcaService _marcaService;
        private readonly ProductoService _productoService;
        private readonly CarritoService _carritoService;
        private readonly OrdenService _ordenService;
        private readonly DireccionService _direccionService;
        private readonly CorreoService _correoService;

        public HomeController(
            ILogger<HomeController> logger,
            CategoriaService categoriaService,
            MarcaService marcaService,
            ProductoService productoService,
            CarritoService carritoService,
            OrdenService ordenService,
            DireccionService direccionService,
            CorreoService correoService)
        {
            _logger = logger;
            _categoriaService = categoriaService;
            _marcaService = marcaService;
            _productoService = productoService;
            _carritoService = carritoService;
            _ordenService = ordenService;
            _direccionService = direccionService;
            _correoService = correoService;
        }

        //El metodo Index se encargara de cargar el catalogo completo, con todas las categorias, marcas y productos disponibles.
        // y ademas tambien cargara los filtros de categorias, marcas y busqueda, con un metodo general para no tener DRY.
        public async Task<IActionResult> Index(int? idCategoria, int? idMarca, string? buscar, string? nombreFiltro)
        {
            //1. Se cargan las listas base de categorias y marcas
            var categorias = await _categoriaService.GetAllAsync();
            var marcas = await _marcaService.GetAllAsync();

            //2. Traemos los productos aplicando los filtros recibidos (si es que se recibieron)
            var productos = await _productoService.GetCatalogAsync(
                    IdCategoria: idCategoria ?? 0,
                    IdMarca: idMarca ?? 0,
                    buscar: buscar
                );

            //3. Armamos un titulo dinamico para mostrar en la vista, dependiendo del filtro aplicado
            string textoFiltro = "Todos los productos";

            if (!string.IsNullOrEmpty(buscar))
            {
                textoFiltro = $"Resultados para '{buscar}'";
            }
            else if (!string.IsNullOrEmpty(nombreFiltro))
            {
                textoFiltro = $"Resultados por: '{nombreFiltro}'";
            }

            //4. Empacamos y enviamos a la vista
            var catalogo = new CatalogoVM
                {
                    Categorias = categorias,
                    Marcas = marcas,
                    Productos = productos,
                    Filtro = textoFiltro
            };

            //5. Retornamos la vista con el catalogo completo y si tiene filtros aplicados, se mostraran en el titulo dinamico
            return View(catalogo);
        }

        //public async Task<IActionResult> FilterByCategory(int id, string name)
        //{
        //    var categorias = await _categoriaService.GetAllAsync();
        //    var marcas = await _marcaService.GetAllAsync();
        //    var productos = await _productoService.GetCatalogAsync(IdCategoria: id);
        //    var catalogo = new CatalogoVM
        //    {
        //        Categorias = categorias,
        //        Marcas = marcas,
        //        Productos = productos,
        //        Filtro = name
        //    };

        //    return View("Index", catalogo);
        //}

        //public async Task<IActionResult> FilterByBrand(int id, string name)
        //{
        //    var categorias = await _categoriaService.GetAllAsync();
        //    var marcas = await _marcaService.GetAllAsync();
        //    var productos = await _productoService.GetCatalogAsync(IdMarca: id);
        //    var catalogo = new CatalogoVM
        //    {
        //        Categorias = categorias,
        //        Marcas = marcas,
        //        Productos = productos,
        //        Filtro = id == 0 ? "Todos los productos" : $"Resultados para {name}"
        //    };

        //    return View("Index", catalogo);
        //}

        //[HttpPost]
        //public async Task<IActionResult> FilterBySearch(string value)
        //{
        //    var categorias = await _categoriaService.GetAllAsync();
        //    var marcas = await _marcaService.GetAllAsync();
        //    var productos = await _productoService.GetCatalogAsync(buscar: value);
        //    var catalogo = new CatalogoVM
        //    {
        //        Categorias = categorias,
        //        Marcas = marcas,
        //        Productos = productos,
        //        Filtro = $"Resultados para {value}"
        //    };

        //    return View("Index", catalogo);
        //}

        public async Task<IActionResult> ProductDetail(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(int productoId, int cantidad)
        {
            var producto = await _productoService.GetByIdAsync(productoId);

            //Delegamos la lógica de agregar al carrito al servicio
            _carritoService.AgregarAlCarrito(producto, cantidad);

            //Respuesta para el usuario
            ViewBag.mensaje = $"{cantidad} unidad(es) de {producto.Nombre} agregada(s) al carrito.";

            return View("ProductDetail", producto);
            //var carrito = HttpContext.Session.GetCarrito<List<CarritoItemVM>>("Carrito") ?? new List<CarritoItemVM>();

            //if(carrito.Find(x => x.IdProducto == productoId) == null)
            //{
            //    carrito.Add(new CarritoItemVM
            //    {
            //        IdProducto = productoId,
            //        Nombre = producto.Nombre,
            //        Precio = producto.Precio,
            //        Cantidad = cantidad
            //    });
            //}
            //else
            //{
            //    var updateProducto = carrito.Find(x => x.IdProducto == productoId);
            //    updateProducto!.Cantidad += cantidad;
            //}

            //HttpContext.Session.SetCarrito("Carrito", carrito);
            //ViewBag.mensaje = $"{cantidad} unidad(es) de {producto.Nombre} agregada(s) al carrito.";
            //return View("ProductDetail", producto);
        }

        public IActionResult ViewCart()
        {
            var carrito = _carritoService.ObtenerCarrito();
            return View(carrito);
        }

        public IActionResult RemoveItemToCart(int productoId)
        {
            _carritoService.EliminarDelCarrito(productoId);
            var carrito = _carritoService.ObtenerCarrito();
            return View("ViewCart", carrito);
        }

        public IActionResult EmptyCart()
        {
            _carritoService.VaciarCarrito();
            var carrito = _carritoService.ObtenerCarrito();
            return View("ViewCart", carrito);
        }

        [HttpPost]
        public async Task<IActionResult> PayNow(DireccionVM direccionEnvio)
        {
            try
            {
                var carrito = _carritoService.ObtenerCarrito();

                //El TODO sirve para ver en TaskList en la consola se accede por el menu view
                int idCliente = 0;
                string correoCliente = "";
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var parsedId))
                {
                    idCliente = parsedId;
                }

                if(emailClaim != null)
                {
                    correoCliente = emailClaim.Value;
                }

                //1. Guardamos la orden para recibir el ID de la orden generada
                int idNuevaVenta = await _ordenService.AddAsync(carrito, idCliente, direccionEnvio);

                //2. Magia del Correo: Generar PDF y enviar
                if(idNuevaVenta > 0 && !string.IsNullOrEmpty(correoCliente))
                {
                    //Creamos el PDF en memoria
                    byte[] pdfBytes = await _ordenService.GenerarReciboPdfAsync(idNuevaVenta, idCliente);

                    //Enviamos el correo con el PDF adjunto
                    if(pdfBytes != null)
                    {
                        await _correoService.EnviarReciboPorCorreoAsync(correoCliente, pdfBytes, idNuevaVenta, carrito);
                    }
                }

                _carritoService.VaciarCarrito();

                return View("SalesCompleted");

            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al procesar el pago. Por favor, inténtelo de nuevo.";
                return View("ViewCart", _carritoService.ObtenerCarrito());
            }
        }

        public IActionResult SalesCompleted() { 
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
